using System;
using System.Collections.Generic;
using System.Text;
using Lilium.Net.IO;

namespace Lilium.Protocol.PacketLib.Packets.Client
{
    public class EncryptionResponsePacket : MCPacket
    {
        public byte[] Key;
        public byte[] Token;
        public EncryptionResponsePacket() { }
        public EncryptionResponsePacket(byte[] key,byte[] token)
        {
            this.Key = key;
            this.Token = token;
        }
        public override void Read(InputBuffer input, int protocol)
        {
            this.Key = input.ReadByteArray(protocol);
            this.Token = input.ReadByteArray(protocol);
        }

        public override void Write(OutputBuffer output, int protocol)
        {
            output.WriteByteArray(this.Key,protocol);
            output.WriteByteArray(this.Token, protocol);
        }
    }
}
