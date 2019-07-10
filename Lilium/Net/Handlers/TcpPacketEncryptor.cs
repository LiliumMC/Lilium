using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.Handlers
{
    class TcpPacketEncryptor: MessageToMessageCodec<IByteBuffer, IByteBuffer>
    {
        private Session session;

        public TcpPacketEncryptor(Session session)
        {
            this.session = session;
        }

        protected override void Decode(IChannelHandlerContext ctx, IByteBuffer msg, List<object> output)
        {
            throw new NotImplementedException();
        }

        protected override void Encode(IChannelHandlerContext ctx, IByteBuffer msg, List<object> output)
        {
            throw new NotImplementedException();
        }
    }
}
