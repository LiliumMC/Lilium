using System;
using System.Collections.Generic;
using System.Text;
using Lilium.Net.IO;

namespace Lilium.Protocol.PacketLib.Packets.Client
{
    class StatusPingPacket : MCPacket
    {
        public long Time;
        public StatusPingPacket() { }
        public StatusPingPacket(long time)
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
