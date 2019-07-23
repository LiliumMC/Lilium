using System;
using System.Collections.Generic;
using System.Text;
using Lilium.Net.IO;

namespace Lilium.Protocol.PacketLib.Packets.Server
{
    class HandshakePacket : Packet
    {
        public int Protocol;
        public string Host;
        public int Port;
        public int Intent;
        public void Read(InputBuffer input)
        {
            this.Protocol = input.ReadVarInt();
            this.Host = input.ReadString();
            this.Port = input.ReadUnsignedShort();
            this.Intent = input.ReadVarInt();
        }

        public void Write(OutputBuffer output)
        {
            throw new NotImplementedException();
        }
    }
}
