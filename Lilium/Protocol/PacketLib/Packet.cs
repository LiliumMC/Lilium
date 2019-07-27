using Lilium.Net.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Protocol.PacketLib
{
    public abstract class Packet
    {
        public abstract void Read(InputBuffer input,int protocol);
        public abstract void Write(OutputBuffer output,int protocol);
        public abstract bool IsPriority { get; }
    }
}
