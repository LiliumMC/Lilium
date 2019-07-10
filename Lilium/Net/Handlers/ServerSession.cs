using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lilium.Net.Handlers
{
    class ServerSession
    {
        private IEventLoopGroup group;
        private IChannel channel;
        public async Task Bind(int inetPort)
        {
            if (this.group != null)
                return;
            this.group = new MultithreadEventLoopGroup();
            Bootstrap bootstrap = new Bootstrap();
            bootstrap
                .Channel<TcpSocketChannel>()
                .Handler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    channel.Configuration.SetOption(ChannelOption.IpTos, 18);
                    channel.Configuration.SetOption(ChannelOption.TcpNodelay, false);

                    IChannelPipeline pipeline = channel.Pipeline;
                }))
                .Group(group).LocalAddress(inetPort);
            channel=await bootstrap.BindAsync();
        }

        public void Close()
        {
            if (this.channel != null)
            {
                if (this.channel.Open)
                {
                    channel.CloseAsync().Wait();
                }
                this.channel = null;
            }
            if (this.group != null)
            {
                group.ShutdownGracefullyAsync();
                group = null;
            }
        }
    }
}
