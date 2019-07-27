using Lilium.Crypto;
using Lilium.Net;
using Lilium.Protocol.Handlers;
using Lilium.Protocol.Message;
using Lilium.Protocol.PacketLib;
using Lilium.Protocol.PacketLib.Version;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Protocol
{
    public class MinecraftProtocol : PacketProtocol
    {
        public HandleStates States { get; set; } = HandleStates.HandShake;
        private MCVersion version;
        private CryptoHandler crypto;

        public MinecraftProtocol() : this(0) { }
        public MinecraftProtocol(int version)
        {
            Protocol = version;
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
        public override void newClientSession(Session session)
        {
            throw new NotImplementedException();
        }
        public void startEncrypt(byte[] key)
        {
            this.crypto = new CryptoHandler(key);
        }
        public void setVersion(int protocol)
        {
            switch (protocol)
            {
                default:
                    version = new MCDefault(this);
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
                case HandleStates.Login:
                    if (client)
                        version.initClientLogin();
                    else
                        version.initServerLogin();
                    break;
                case HandleStates.Game:
                    if (client)
                        version.initClientGame();
                    else
                        version.initServerGame();
                    break;
                case HandleStates.Status:
                    if (client)
                        version.initClientStatus();
                    else
                        version.initServerStatus();
                    break;
            }
            States = states;
        }
    }
}
