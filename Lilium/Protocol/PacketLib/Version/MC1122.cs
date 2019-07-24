using Lilium.Protocol.PacketLib.Packets.Client;
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
            protocol.RegisterOutgoing<StatusQueryPacket>(0x00);
            protocol.RegisterOutgoing<StatusPingPacket>(0x00);

            protocol.RegisterIncoming<StatusResponsePacket>(0x00);
            protocol.RegisterIncoming<StatusPongPacket>(0x01);
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
            protocol.RegisterIncoming<StatusQueryPacket>(0x00);
            protocol.RegisterIncoming<StatusPingPacket>(0x01);

            protocol.RegisterOutgoing<StatusResponsePacket>(0x00);
            protocol.RegisterOutgoing<StatusPongPacket>(0x01);
        }
    }
}
