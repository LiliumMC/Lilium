using Lilium.Net.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Protocol.PacketLib
{
    public interface Packet
    {
        void Read(InputBuffer input);
        void Write(OutputBuffer output);
        bool IsPriority { get; }
    }
}
