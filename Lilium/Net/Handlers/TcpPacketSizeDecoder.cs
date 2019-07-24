using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Lilium.Net.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.Handlers
{
    class TcpPacketSizeDecoder : ByteToMessageDecoder
    {
        private Session session;
        public TcpPacketSizeDecoder(Session session)
        {
            this.session = session;
        }
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            input.MarkReaderIndex();
            byte[] buf = new byte[5];
            for (int i = 0; i < buf.Length; i++)
            {
                if (!input.IsReadable())
                {
                    input.ResetReaderIndex();
                    return;
                }
                buf[i] = input.ReadByte();
                if (buf[i] >= 0)
                {
                    int size = new InputBuffer(Unpooled.WrappedBuffer(buf)).ReadVarInt();
                    if (input.ReadableBytes < size)
                    {
                        input.ResetReaderIndex();
                        return;
                    }
                    output.Add(input.ReadBytes(size));
                    return;
                }
            }
        }
    }
}
