using System;
using System.Collections.Generic;
using System.Text;
using Lilium.Net.IO;

namespace Lilium.Protocol.PacketLib.Packets.Server
{
    class StatusPongPacket : MCPacket
    {
        public long Time;
        public StatusPongPacket(long time)
        {
            this.Time = time;
        }
        public override void Read(InputBuffer input)
        {
            this.Time = input.ReadLong();
        }

        public override void Write(OutputBuffer output)
        {
            output.WriteLong(this.Time);
        }
    }
}
