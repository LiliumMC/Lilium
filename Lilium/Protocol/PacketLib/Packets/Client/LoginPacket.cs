using System;
using System.Collections.Generic;
using System.Text;
using Lilium.Net.IO;

namespace Lilium.Protocol.PacketLib.Packets.Client
{
    class LoginPacket : Packet
    {
        public string Name;
        public LoginPacket() { }
        public LoginPacket(string name)
        {
            this.Name = name;
        }

        public bool IsPriority { get
            {
                return true;
            } }

        public void Read(InputBuffer input)
        {
            this.Name = input.ReadString();
        }

        public void Write(OutputBuffer output)
        {
            output.WriteString(this.Name);
        }
    }
}
