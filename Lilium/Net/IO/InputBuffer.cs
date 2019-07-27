using DotNetty.Buffers;
using Lilium.Protocol.PacketLib.Version;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.IO
{
    public class InputBuffer
    {
        IByteBuffer buf;
        public int ReadableBytes
        {
            get
            {
                return buf.ReadableBytes;
            }
        }
        public InputBuffer(IByteBuffer buf)
        {
            this.buf = buf;
        }
        public byte ReadByte()
        {
            return buf.ReadByte();
        }
        public int ReadVarInt()
        {
            int i = 0;
            int j = 0;
            int k = 0;
            while (true)
            {
                k = ReadByte();
                i |= (k & 0x7F) << j++ * 7;
                if (j > 5) throw new OverflowException("VarInt too big");
                if ((k & 0x80) != 128) break;
            }
            return i;
        }
        public byte[] ReadData(int length)
        {
            if (length > 0)
            {
                byte[] cache = new byte[length];
                buf.ReadBytes(cache);
                return cache;
            }
            return new byte[] { };
        }
        public IByteBuffer ReadBytes(int length)
        {
            return buf.ReadBytes(length);
        }
        public string ReadString()
        {
            int len = ReadVarInt();
            return Encoding.UTF8.GetString(ReadData(len));
        }
        public short ReadShort()
        {
            return buf.ReadShort();
        }
        public long ReadLong()
        {
            return buf.ReadLong();
        }
        public int ReadUnsignedShort()
        {
            return buf.ReadShort() & 0xFFFF;
        }
        public byte[] ReadByteArray(int protocol)
        {
            int len = protocol >= MCVersion.MC18Version
                ? this.ReadVarInt()
                : this.ReadShort();
            return ReadData(len);
        }
    }
}
