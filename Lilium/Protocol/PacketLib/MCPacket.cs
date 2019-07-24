using System;
using System.Collections.Generic;
using System.Text;
using Lilium.Net.IO;

namespace Lilium.Protocol.PacketLib
{
    public abstract class MCPacket:Packet
    {
        public bool IsPriority { get
            {
                return false;
            } }

        public abstract void Read(InputBuffer input);

        public abstract void Write(OutputBuffer output);
    }
}
