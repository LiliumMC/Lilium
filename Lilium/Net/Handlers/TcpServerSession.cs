using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Lilium.Protocol.Message;
using Lilium.Protocol.PacketLib;

namespace Lilium.Net.Handlers
{
    class TcpServerSession : TcpSession
    {
        private HandleServer server;
        public TcpServerSession(string host,int port,PacketProtocol type, HandleServer server):base(host,port,type)
        {
            this.server = server;
        }
        public override void ChannelActive(IChannelHandlerContext ctx)
        {
            base.ChannelActive(ctx);
            this.server.AddSession(this);
        }
        public override void ChannelInactive(IChannelHandlerContext context)
        {
            base.ChannelInactive(context);
            this.server.RemoveSession(this);
        }
    }
}
