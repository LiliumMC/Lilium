using System;
using System.Collections.Generic;
using System.Text;
using Lilium.Net.IO;

namespace Lilium.Protocol.PacketLib.Packets.Client
{
    class HandshakePacket : Packet
    {
        public int ProtocolNum;
        public string Host;
        public int Port;
        public int Intent;

        public override bool IsPriority { get
            {
                return true;
            } }

        public override void Read(InputBuffer input, int protocol)
        {
            this.ProtocolNum = input.ReadVarInt();
            this.Host = input.ReadString();
            this.Port = input.ReadUnsignedShort();
            this.Intent = input.ReadVarInt();
        }

        public override void Write(OutputBuffer output, int protocol)
        {
            throw new NotImplementedException();
        }
    }
}
