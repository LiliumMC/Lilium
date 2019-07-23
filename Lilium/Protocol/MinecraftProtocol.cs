using Lilium.Crypto;
using Lilium.Net;
using Lilium.Protocol.Handlers;
using Lilium.Protocol.Messge;
using Lilium.Protocol.PacketLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Protocol
{
    class MinecraftProtocol : PacketProtocol
    {
        public HandleStates States { get; set; } = HandleStates.HandShake;
        private CryptoHandler crypto;
        public override CryptoHandler getCrypto()
        {
            return this.crypto;
        }

        public override void newServerSession(Session session)
        {
            session.AddListener(new ServerListener());
        }

        public void startEncrypt(byte[] key)
        {
            this.crypto = new CryptoHandler(key);
        }
    }
}
