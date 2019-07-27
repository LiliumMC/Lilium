using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Lilium.Crypto
{
    public class Cipher
    {
        AsymmetricCipherKeyPair keyPair;
        public AsymmetricKeyParameter PublicKey
        {
            get
            {
                return keyPair.Public;
            }
        }
        public AsymmetricKeyParameter PrivateKey
        {
            get
            {
                return keyPair.Private;
            }
        }
        public Cipher()
        {
            RsaKeyPairGenerator keyGenerator = new RsaKeyPairGenerator();
            KeyGenerationParameters param = new RsaKeyGenerationParameters(
                Org.BouncyCastle.Math.BigInteger.ValueOf(3),
                new SecureRandom(), 1024, 25);
            keyGenerator.Init(param);
            keyPair = keyGenerator.GenerateKeyPair();
        }
        public byte[] getPublic()
        {
            TextWriter textWriter = new StringWriter();
            PemWriter pemWriter = new PemWriter(textWriter);
            pemWriter.WriteObject(PublicKey);
            string public_key = textWriter.ToString();
            Regex regex = new Regex(@"-+BEGIN PUBLIC KEY-+\r?\n(.*)\r?\n-+END PUBLIC KEY-+", RegexOptions.Singleline);
            public_key = regex.Match(public_key).Result("$1");
            return Convert.FromBase64String(@public_key);
        }
        public byte[] Decrypt(byte[] data)
        {
            var engine = new RsaEngine();
            engine.Init(false, PrivateKey);
            return engine.ProcessBlock(data, 0, data.Length);
        }
        public static byte[] Encrypt(byte[] data, AsymmetricKeyParameter pubKey)
        {
            var cipher = new RsaEngine();
            cipher.Init(true, pubKey);
            return cipher.ProcessBlock(data, 0, data.Length);
        }
        public static RSACryptoServiceProvider DecodeRSAPublicKey(byte[] x509key)
        {
            /* Code from StackOverflow no. 18091460 */

            byte[] SeqOID = { 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01 };

            System.IO.MemoryStream ms = new System.IO.MemoryStream(x509key);
            System.IO.BinaryReader reader = new System.IO.BinaryReader(ms);

            if (reader.ReadByte() == 0x30)
                ReadASNLength(reader); //skip the size
            else
                return null;

            int identifierSize = 0; //total length of Object Identifier section
            if (reader.ReadByte() == 0x30)
                identifierSize = ReadASNLength(reader);
            else
                return null;

            if (reader.ReadByte() == 0x06) //is the next element an object identifier?
            {
                int oidLength = ReadASNLength(reader);
                byte[] oidBytes = new byte[oidLength];
                reader.Read(oidBytes, 0, oidBytes.Length);
                if (oidBytes.SequenceEqual(SeqOID) == false) //is the object identifier rsaEncryption PKCS#1?
                    return null;

                int remainingBytes = identifierSize - 2 - oidBytes.Length;
                reader.ReadBytes(remainingBytes);
            }

            if (reader.ReadByte() == 0x03) //is the next element a bit string?
            {
                ReadASNLength(reader); //skip the size
                reader.ReadByte(); //skip unused bits indicator
                if (reader.ReadByte() == 0x30)
                {
                    ReadASNLength(reader); //skip the size
                    if (reader.ReadByte() == 0x02) //is it an integer?
                    {
                        int modulusSize = ReadASNLength(reader);
                        byte[] modulus = new byte[modulusSize];
                        reader.Read(modulus, 0, modulus.Length);
                        if (modulus[0] == 0x00) //strip off the first byte if it's 0
                        {
                            byte[] tempModulus = new byte[modulus.Length - 1];
                            Array.Copy(modulus, 1, tempModulus, 0, modulus.Length - 1);
                            modulus = tempModulus;
                        }

                        if (reader.ReadByte() == 0x02) //is it an integer?
                        {
                            int exponentSize = ReadASNLength(reader);
                            byte[] exponent = new byte[exponentSize];
                            reader.Read(exponent, 0, exponent.Length);

                            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                            RSAParameters RSAKeyInfo = new RSAParameters();
                            RSAKeyInfo.Modulus = modulus;
                            RSAKeyInfo.Exponent = exponent;
                            RSA.ImportParameters(RSAKeyInfo);
                            return RSA;
                        }
                    }
                }
            }
            return null;
        }
        private static int ReadASNLength(BinaryReader reader)
        {
            //Note: this method only reads lengths up to 4 bytes long as
            //this is satisfactory for the majority of situations.
            int length = reader.ReadByte();
            if ((length & 0x00000080) == 0x00000080) //is the length greater than 1 byte
            {
                int count = length & 0x0000000f;
                byte[] lengthBytes = new byte[4];
                reader.Read(lengthBytes, 4 - count, count);
                Array.Reverse(lengthBytes); //
                length = BitConverter.ToInt32(lengthBytes, 0);
            }
            return length;
        }
    }
}
