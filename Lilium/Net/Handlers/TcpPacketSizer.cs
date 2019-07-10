using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.Handlers
{
    class TcpPacketSizer : MessageToMessageCodec<IByteBuffer, OutputBuffer>
    {
        private Session session;
        public TcpPacketSizer(Session session)
        {
            this.session = session;
        }
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            Debug.Log("Handle Packet");
            InputBuffer ib = new InputBuffer(input);
            int size = ib.ReadVarInt();
            InputBuffer packetData = new InputBuffer(ib.ReadBytes(size));
            output.Add(packetData.ReadData(packetData.ReadableBytes));
        }

        protected override void Encode(IChannelHandlerContext ctx, OutputBuffer msg, List<object> output)
        {
            IByteBuffer buf = Unpooled.Buffer();
            OutputBuffer cache = new OutputBuffer(buf);
            int length = msg.ReadableBytes;
            cache.WriteVarInt(length);
            cache.WriteBytes(msg.getBuffer());
            output.Add(buf);
        }
    }
}
