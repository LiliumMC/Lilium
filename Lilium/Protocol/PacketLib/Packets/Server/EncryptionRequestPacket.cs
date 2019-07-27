using System;
using System.Collections.Generic;
using System.Text;
using Lilium.Net.IO;
using Lilium.Protocol.PacketLib.Version;

namespace Lilium.Protocol.PacketLib.Packets.Server
{
    public class EncryptionRequestPacket : MCPacket
    {
        public string ServerID;
        public byte[] PublicKey;
        public byte[] Token;
        public EncryptionRequestPacket(string serverID,byte[] pubkey,byte[] token)
        {
            this.ServerID = serverID;
            this.PublicKey = pubkey;
            this.Token = token;
        }
        public override void Read(InputBuffer input, int protocol)
        {
            this.ServerID = input.ReadString();
            this.PublicKey = input.ReadByteArray(protocol);
            this.Token = input.ReadByteArray(protocol);
        }

        public override void Write(OutputBuffer output, int protocol)
        {
            output.WriteString(ServerID);
            output.WriteByteArray(PublicKey,protocol);
            output.WriteByteArray(Token, protocol);
        }
    }
}
