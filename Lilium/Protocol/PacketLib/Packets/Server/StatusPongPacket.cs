using System;
using System.Collections.Generic;
using System.Text;
using Lilium.Net.IO;

namespace Lilium.Protocol.PacketLib.Packets.Server
{
    public class StatusPongPacket : MCPacket
    {
        public long Time;
        public StatusPongPacket(long time)
        {
            this.Time = time;
        }
        public override void Read(InputBuffer input, int protocol)
        {
            this.Time = input.ReadLong();
        }

        public override void Write(OutputBuffer output, int protocol)
        {
            output.WriteLong(this.Time);
        }
    }
}
