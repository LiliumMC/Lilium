using System;
using System.Collections.Generic;
using System.Text;
using Lilium.Net.IO;

namespace Lilium.Protocol.PacketLib.Packets.Client
{
    class StatusQueryPacket : Packet
    {
        public override bool IsPriority { get
            {
                return true;
            } }

        public override void Read(InputBuffer input, int protocol)
        {
        }

        public override void Write(OutputBuffer output, int protocol)
        {
        }
    }
}
