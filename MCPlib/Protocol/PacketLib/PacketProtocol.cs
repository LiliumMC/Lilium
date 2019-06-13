using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPlib.Protocol.PacketLib
{
    abstract class PacketProtocol
    {
        public abstract int protocolVersion { get; }
        public abstract Protocol.PacketIncomingType getPacketIncomingType(int packetID);
        public abstract Protocol.PacketOutgoingType getPacketOutgoingType(int packetID);
        public abstract int getPacketIncomingID(Protocol.PacketIncomingType packet);
        public abstract int getPacketOutgoingID(Protocol.PacketOutgoingType packet);

    }
}
