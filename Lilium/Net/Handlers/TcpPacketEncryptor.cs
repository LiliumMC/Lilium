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
            if (this.session.getPacketProtocol().getCrypto() != null)
            {
                int length = msg.ReadableBytes;
                byte[] bytes = getBytes(msg);
                IByteBuffer result =ctx.Allocator.HeapBuffer(this.session.getPacketProtocol().getCrypto().getDecryptOutputSize(length));
                result.SetWriterIndex(this.session.getPacketProtocol().getCrypto().decrypt(bytes, 0, length, result.Array, result.ArrayOffset));
                output.Add(result);
            }
            else
            {
                output.Add(msg.ReadBytes(msg.ReadableBytes));
            }
        }

        protected override void Encode(IChannelHandlerContext ctx, IByteBuffer msg, List<object> output)
        {
            if (this.session.getPacketProtocol().getCrypto() != null)
            {
                int length = msg.ReadableBytes;
                byte[] bytes = getBytes(msg);
                int outLength = this.session.getPacketProtocol().getCrypto().getEncryptOutputSize(length);
                IByteBuffer result = ctx.Allocator.HeapBuffer(outLength);
                result.SetWriterIndex(this.session.getPacketProtocol().getCrypto().encrypt(bytes, 0, length, result.Array, result.ArrayOffset));
                output.Add(result);
            }
            else
            {
                output.Add(msg.ReadBytes(msg.ReadableBytes));
            }
        }
        private byte[] getBytes(IByteBuffer buf)
        {
            int length = buf.ReadableBytes;
            byte[] data = new byte[length];
            buf.ReadBytes(data, 0, length);
            return data;
        }
    }
}
