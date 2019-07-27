using System;
using System.Collections.Generic;
using System.Text;
using Lilium.Net.IO;

namespace Lilium.Protocol.PacketLib.Packets.Client
{
    public class LoginPacket : Packet
    {
        public string Name;
        public LoginPacket() { }
        public LoginPacket(string name)
        {
            this.Name = name;
        }

        public override bool IsPriority { get
            {
                return true;
            } }

        public override void Read(InputBuffer input, int protocol)
        {
            this.Name = input.ReadString();
        }

        public override void Write(OutputBuffer output, int protocol)
        {
            output.WriteString(this.Name);
        }
    }
}
