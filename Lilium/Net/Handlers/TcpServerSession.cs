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
        private Server server;
        public TcpServerSession(string host,int port,PacketProtocol type,Server server):base(host,port,type)
        {
            this.server = server;
        }
        public override void ChannelActive(IChannelHandlerContext ctx)
        {
            base.ChannelActive(ctx);
        }
    }
}
