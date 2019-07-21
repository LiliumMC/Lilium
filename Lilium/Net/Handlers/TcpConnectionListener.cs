using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Lilium.Protocol.PacketLib;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Lilium.Net.Handlers
{
    class TcpConnectionListener:ConnectionListener
    {
        string host;
        int port;
        Server server;

        private IEventLoopGroup group;
        private IChannel channel;

        public TcpConnectionListener(string host,int port,Server server)
        {
            this.host = host;
            this.port = port;
            this.server = server;
        }
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
                    IPEndPoint address = (IPEndPoint)channel.RemoteAddress;
                    PacketProtocol protocol = server.createPacketProtocol();
                    TcpSession session = new TcpServerSession(address.Address.ToString(), address.Port, protocol, server);

                    channel.Configuration.SetOption(ChannelOption.IpTos, 18);
                    channel.Configuration.SetOption(ChannelOption.TcpNodelay, false);

                    IChannelPipeline pipeline = channel.Pipeline;
                    pipeline.AddLast("crypto", new TcpPacketEncryptor(session));
                    pipeline.AddLast("sizer", new TcpPacketSizer(session));
                    pipeline.AddLast("codec", new TcpPacketCodec(session));
                    pipeline.AddLast("manager", session);

                }))
                .Group(group).LocalAddress(inetPort);
            channel=await bootstrap.BindAsync();
        }

        public async Task Close()
        {
            if (this.channel != null)
            {
                if (this.channel.Open)
                {
                    await channel.CloseAsync();
                }
                this.channel = null;
            }
            if (this.group != null)
            {
                await group.ShutdownGracefullyAsync();
                group = null;
            }
        }

        public string getHost()
        {
            return this.host;
        }

        public int getPort()
        {
            return this.port;
        }
    }
}
