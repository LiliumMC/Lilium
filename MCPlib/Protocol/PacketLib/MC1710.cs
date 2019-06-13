using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPlib.Protocol.PacketLib
{
    class MC1710 : PacketProtocol
    {
        public override int protocolVersion { get
            {
                return 5;
            } }
        public override Protocol.PacketIncomingType getPacketIncomingType(int packetID)
        {
            switch (packetID)
            {
                case 0x00:return Protocol.PacketIncomingType.KeepAlive;
                case 0x01:return Protocol.PacketIncomingType.JoinGame;
                case 0x02:return Protocol.PacketIncomingType.ChatMessage;
                default:return Protocol.PacketIncomingType.UnknownPacket;
            }
        }
        public override int getPacketIncomingID(Protocol.PacketIncomingType packet)
        {
            switch (packet)
            {
                case Protocol.PacketIncomingType.KeepAlive: return 0x00;
                case Protocol.PacketIncomingType.JoinGame: return 0x01;
                case Protocol.PacketIncomingType.ChatMessage: return 0x02;
                default: return 0xFF;
            }
        }

        public override int getPacketOutgoingID(Protocol.PacketOutgoingType packet)
        {
            switch (packet)
            {
                case Protocol.PacketOutgoingType.KeepAlive:return 0x00;
                case Protocol.PacketOutgoingType.ChatMessage:return 0x01;
                default:return 0xFF;
            }
        }

        public override Protocol.PacketOutgoingType getPacketOutgoingType(int packetID)
        {
            switch (packetID)
            {
                case 0x00:return Protocol.PacketOutgoingType.KeepAlive;
                case 0x01:return Protocol.PacketOutgoingType.ChatMessage;
                default:return Protocol.PacketOutgoingType.Unknown;
            }
        }
    }
}
