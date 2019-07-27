using Lilium.Protocol.PacketLib.Packets.Client;
using Lilium.Protocol.PacketLib.Packets.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Protocol.PacketLib.Version
{
    public class MCDefault : MCVersion
    {
        PacketProtocol protocol;
        public MCDefault(PacketProtocol protocol)
        {
            this.protocol = protocol;
        }
        public override void initClientGame()
        {
            throw new NotImplementedException();
        }

        public override void initClientHandshake()
        {
            protocol.RegisterOutgoing<HandshakePacket>(0x00);
        }

        public override void initClientLogin()
        {
            protocol.RegisterOutgoing<LoginPacket>(0x00);
            protocol.RegisterOutgoing<EncryptionResponsePacket>(0x01);

            protocol.RegisterIncoming<EncryptionRequestPacket>(0x01);
        }

        public override void initClientStatus()
        {
            protocol.RegisterOutgoing<StatusQueryPacket>(0x00);
            protocol.RegisterOutgoing<StatusPingPacket>(0x00);

            protocol.RegisterIncoming<StatusResponsePacket>(0x00);
            protocol.RegisterIncoming<StatusPongPacket>(0x01);
        }

        public override void initServerGame()
        {
            throw new NotImplementedException();
        }

        public override void initServerHandshake()
        {
            protocol.RegisterIncoming<HandshakePacket>(0x00);
        }

        public override void initServerLogin()
        {
            protocol.RegisterIncoming<LoginPacket>(0x00);
            protocol.RegisterIncoming<EncryptionResponsePacket>(0x01);

            protocol.RegisterOutgoing<EncryptionRequestPacket>(0x01);
        }

        public override void initServerStatus()
        {
            protocol.RegisterIncoming<StatusQueryPacket>(0x00);
            protocol.RegisterIncoming<StatusPingPacket>(0x01);

            protocol.RegisterOutgoing<StatusResponsePacket>(0x00);
            protocol.RegisterOutgoing<StatusPongPacket>(0x01);
        }
    }
}
