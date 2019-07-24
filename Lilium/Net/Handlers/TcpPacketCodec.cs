using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Lilium.Net.Event;
using Lilium.Net.IO;
using Lilium.Protocol.PacketLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.Handlers
{
    class TcpPacketCodec:MessageToMessageCodec<IByteBuffer, Packet>
    {
        private Session session;

        public TcpPacketCodec(Session session)
        {
            this.session = session;
        }

        protected override void Decode(IChannelHandlerContext ctx, IByteBuffer msg, List<object> output)
        {
            InputBuffer input = new InputBuffer(msg);
            int id = input.ReadVarInt();
            Packet packet = this.session.getPacketProtocol().createIncomingPacket(id);
            packet.Read(input);

            if (packet.IsPriority)
                this.session.CallEvent(new PacketReceivedEvent(session, packet));
            else
                output.Add(packet);
        }

        protected override void Encode(IChannelHandlerContext ctx, Packet packet, List<object> output)
        {
            OutputBuffer buf = new OutputBuffer(Unpooled.Buffer());
            buf.WriteVarInt(session.getPacketProtocol().getOutgoingID(packet));
            packet.Write(buf);
            output.Add(buf.getBuffer());
        }
    }
}
