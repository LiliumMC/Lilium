using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Text;

namespace Lilium.Crypto
{
    public class CryptoHandler
    {
        BufferedBlockCipher dec;
        BufferedBlockCipher enc;
        public CryptoHandler(byte[] key)
        {
            var coder = new AesEngine();
            CfbBlockCipher cfbCipher = new CfbBlockCipher(coder, 8);
            this.dec = new BufferedBlockCipher(cfbCipher);
            this.dec.Init(false, new KeyParameter(key));
            this.enc = new BufferedBlockCipher(cfbCipher);
            this.enc.Init(true, new KeyParameter(key));
        }
        public int getDecryptOutputSize(int len)
        {
            return this.dec.GetOutputSize(len);
        }
        public int getEncryptOutputSize(int len)
        {
            return this.enc.GetOutputSize(len);
        }

        public int decrypt(byte[] input, int inputOffset, int inputLength, byte[] output, int outputOffset)
        {
            return dec.ProcessBytes(input, inputOffset, inputLength, output, outputOffset);
        }
        public int encrypt(byte[] input, int inputOffset, int inputLength, byte[] output, int outputOffset)
        {
            return enc.ProcessBytes(input, inputOffset, inputLength, output, outputOffset);
        }
    }
}
