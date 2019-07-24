using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.IO
{
    public class OutputBuffer
    {
        IByteBuffer buf;
        public int ReadableBytes
        {
            get
            {
                return buf.ReadableBytes;
            }
        }
        public OutputBuffer(IByteBuffer buf)
        {
            this.buf = buf;
        }
        public void WriteByte(int b)
        {
            buf.WriteByte(b);
        }
        public void WriteData(byte[] b)
        {
            buf.WriteBytes(b);
        }
        public void WriteBytes(IByteBuffer buf)
        {
            this.buf.WriteBytes(buf);
        }
        public void WriteVarInt(int paramInt)
        {
            while ((paramInt & -128) != 0)
            {
                WriteByte(paramInt & 127 | 128);
                paramInt = (int)(((uint)paramInt) >> 7);
            }
            WriteByte(paramInt);
        }
        public void WriteString(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            WriteVarInt(bytes.Length);
            WriteData(bytes);
        }
        public void WriteShort(int param)
        {
            buf.WriteShort(param);
        }
        public void WriteLong(long param)
        {
            buf.WriteLong(param);
        }
        public IByteBuffer getBuffer()
        {
            return buf;
        }
    }
}
