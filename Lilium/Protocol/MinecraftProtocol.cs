using Lilium.Crypto;
using Lilium.Protocol.PacketLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Protocol
{
    class MinecraftProtocol : PacketProtocol
    {
        private CryptoHandler crypto;
        public override CryptoHandler getCrypto()
        {
            return this.crypto;
        }

        public void startEncrypt(byte[] key)
        {
            this.crypto = new CryptoHandler(key);
        }
    }
}
