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
            throw new NotImplementedException();
        }
    }
}
