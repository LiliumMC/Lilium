using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Lilium.Net.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.Handlers
{
    class TcpPacketSizePrepender:MessageToByteEncoder<IByteBuffer>
    {
        private Session session;
        public TcpPacketSizePrepender(Session session)
        {
            this.session = session;
        }

        protected override void Encode(IChannelHandlerContext context, IByteBuffer msg, IByteBuffer output)
        {
            IByteBuffer buf = context.Allocator.Buffer();
            OutputBuffer cache = new OutputBuffer(buf);
            int length = msg.ReadableBytes;
            cache.WriteVarInt(length);
            cache.WriteBytes(msg.ReadBytes(length));
            output.WriteBytes(cache.getBuffer());
        }
    }
}
