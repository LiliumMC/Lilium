using System;
using System.Collections.Generic;
using System.Text;
using Lilium.Config;
using Lilium.Net.IO;
using Lilium.Protocol.Data.Status;

namespace Lilium.Protocol.PacketLib.Packets.Server
{
    public class StatusResponsePacket : MCPacket
    {
        StatusInfo Info;
        public StatusResponsePacket(StatusInfo info)
        {
            this.Info = info;
        }
        public override void Read(InputBuffer input, int protocol)
        {
            throw new NotImplementedException();
        }

        public override void Write(OutputBuffer output, int protocol)
        {
            output.WriteString(Info.ToString());
        }
    }
}
