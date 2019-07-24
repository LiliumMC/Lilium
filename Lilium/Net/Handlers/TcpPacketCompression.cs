using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Lilium.Net.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.Handlers
{
    class TcpPacketCompression: MessageToMessageCodec<IByteBuffer,IByteBuffer>
    {
        private Session session;
        public TcpPacketCompression(Session session)
        {
            this.session = session;
        }

        protected override void Decode(IChannelHandlerContext ctx, IByteBuffer input, List<object> output)
        {
            InputBuffer packetData = new InputBuffer(input);
            int sizeUncompressed = packetData.ReadVarInt();
            if (sizeUncompressed != 0)
            {
                byte[] toDecompress = packetData.ReadData(packetData.ReadableBytes);
                byte[] uncompressed = ZlibUtils.Decompress(toDecompress, sizeUncompressed);
                output.Add(uncompressed);
                return;
            }
            else
                output.Add(input.ReadBytes(input.ReadableBytes));
        }

        protected override void Encode(IChannelHandlerContext ctx, IByteBuffer msg, List<object> output)
        {
            OutputBuffer buf = new OutputBuffer(ctx.Allocator.Buffer());
            if (msg.ReadableBytes >= session.CompressionTreshold)
            {
                byte[] uncompressed = new byte[msg.ReadableBytes];
                msg.ReadBytes(uncompressed);
                byte[] compressed_packet = ZlibUtils.Compress(uncompressed);
                buf.WriteVarInt(uncompressed.Length);
                buf.WriteData(compressed_packet);
            }
            else
            {
                buf.WriteVarInt(0);
                buf.WriteBytes(msg);
            }
            output.Add(buf.getBuffer());
        }
    }
}
