using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Lilium.Crypto
{
    class CryptoHandler
    {
        ICryptoTransform dec;
        ICryptoTransform enc;
        public CryptoHandler(byte[] key)
        {
            SymmetricAlgorithm  des = Aes.Create();
            des.Key = key;
            des.Mode = CipherMode.CFB;
            des.Padding = PaddingMode.None;
            enc = des.CreateEncryptor();
            dec = des.CreateDecryptor();
        }

        public int decrypt(byte[] input, int inputOffset, int inputLength, byte[] output, int outputOffset)
        {
            return dec.TransformBlock(input, inputOffset, inputLength, output, outputOffset);
        }
        public int encrypt(byte[] input, int inputOffset, int inputLength, byte[] output, int outputOffset)
        {
            return enc.TransformBlock(input, inputOffset, inputLength, output, outputOffset);
        }
    }
}
