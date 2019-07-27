using System;
using System.Collections.Generic;
using System.Text;
using Lilium.Net.IO;

namespace Lilium.Protocol.PacketLib
{
    public abstract class MCPacket:Packet
    {
        public override bool IsPriority { get
            {
                return false;
            } }
        
        public override abstract void Read(InputBuffer input, int protocol);

        public override abstract void Write(OutputBuffer output, int protocol);
    }
}
