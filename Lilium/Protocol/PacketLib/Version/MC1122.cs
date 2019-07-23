using Lilium.Protocol.PacketLib.Packets.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Protocol.PacketLib.Version
{
    public class MC1122 : IMCVersion
    {
        PacketProtocol protocol;
        public MC1122(PacketProtocol protocol)
        {
            this.protocol = protocol;
        }
        public void initClientGame()
        {
            throw new NotImplementedException();
        }

        public void initClientHandshake()
        {
            protocol.RegisterOutgoing<HandshakePacket>(0x00);
        }

        public void initClientLogin()
        {
            throw new NotImplementedException();
        }

        public void initClientStatus()
        {
            throw new NotImplementedException();
        }

        public void initServerGame()
        {
            throw new NotImplementedException();
        }

        public void initServerHandshake()
        {
            protocol.RegisterIncoming<HandshakePacket>(0x00);
        }

        public void initServerLogin()
        {
            throw new NotImplementedException();
        }

        public void initServerStatus()
        {
            throw new NotImplementedException();
        }
    }
}
