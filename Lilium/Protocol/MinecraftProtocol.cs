using Lilium.Crypto;
using Lilium.Net;
using Lilium.Protocol.Handlers;
using Lilium.Protocol.Messge;
using Lilium.Protocol.PacketLib;
using Lilium.Protocol.PacketLib.Version;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Protocol
{
    class MinecraftProtocol : PacketProtocol
    {
        public HandleStates States { get; set; } = HandleStates.HandShake;
        private IMCVersion version;
        private CryptoHandler crypto;
        public MinecraftProtocol(int version)
        {
            setVersion(version);
        }
        public override CryptoHandler getCrypto()
        {
            return this.crypto;
        }

        public override void newServerSession(Session session)
        {
            this.setProtocolState(HandleStates.HandShake, false);
            session.AddListener(new ServerListener());
        }

        public void startEncrypt(byte[] key)
        {
            this.crypto = new CryptoHandler(key);
        }
        public void setVersion(int protocol)
        {
            switch (protocol)
            {
                case 340:
                    version = new MC1122(this);
                    break;
                default:
                    version = new MC1122(this);
                    break;
            }
        }
        public void setProtocolState(HandleStates states,bool client)
        {
            ClearPackets();
            switch (states)
            {
                case HandleStates.HandShake:
                    if (client)
                        version.initClientHandshake();
                    else
                        version.initServerHandshake();
                    break;
            }
            States = states;
        }
    }
}
