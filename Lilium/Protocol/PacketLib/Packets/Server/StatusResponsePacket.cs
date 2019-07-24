using System;
using System.Collections.Generic;
using System.Text;
using Lilium.Config;
using Lilium.Net.IO;
using Lilium.Protocol.Data.Status;

namespace Lilium.Protocol.PacketLib.Packets.Server
{
    class StatusResponsePacket : MCPacket
    {
        StatusInfo Info;
        public StatusResponsePacket(StatusInfo info)
        {
            this.Info = info;
        }
        public override void Read(InputBuffer input)
        {
            throw new NotImplementedException();
        }

        public override void Write(OutputBuffer output)
        {
            output.WriteString(Info.ToString());
        }
    }
}
