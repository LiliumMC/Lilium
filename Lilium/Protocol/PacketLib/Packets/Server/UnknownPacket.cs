using System;
using System.Collections.Generic;
using System.Text;
using Lilium.Net.IO;

namespace Lilium.Protocol.PacketLib.Packets.Server
{
    public class UnknownPacket : Packet
    {
        public byte[] Data;

        public override bool IsPriority { get
            {
                return false;
            } }

        public override void Read(InputBuffer input, int protocol)
        {
            this.Data = input.ReadData(input.ReadableBytes);
        }

        public override void Write(OutputBuffer output, int protocol)
        {
        }
    }
}
