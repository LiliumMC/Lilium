using System;
using System.Collections.Generic;
using System.Text;
using Lilium.Net.IO;

namespace Lilium.Protocol.PacketLib.Packets.Client
{
    class StatusQueryPacket : Packet
    {
        public bool IsPriority { get
            {
                return true;
            } }

        public void Read(InputBuffer input)
        {
            //throw new NotImplementedException();
        }

        public void Write(OutputBuffer output)
        {
            //throw new NotImplementedException();
        }
    }
}
