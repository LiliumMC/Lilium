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
        HandleServer server;

        private IEventLoopGroup group;
        private IChannel channel;

        public TcpConnectionListener(string host,int port, HandleServer server)
        {
            this.host = host;
            this.port = port;
            this.server = server;
        }

        public bool isListening { get
            {
                return this.channel != null && this.channel.Open;
            } }

        public async Task Bind()
        {
            if (this.group != null)
                return;
            this.group = new MultithreadEventLoopGroup();
            ServerBootstrap bootstrap = new ServerBootstrap();
            bootstrap
                .Channel<TcpServerSocketChannel>()
                .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                {
                    IPEndPoint address = (IPEndPoint)channel.RemoteAddress;
                    PacketProtocol protocol = server.CreatePacketProtocol();
                    TcpSession session = new TcpServerSession(address.Address.ToString(), address.Port, protocol, server);
                    session.getPacketProtocol().newServerSession(session);

                    channel.Configuration.SetOption(ChannelOption.IpTos, 18);
                    channel.Configuration.SetOption(ChannelOption.TcpNodelay, false);

                    IChannelPipeline pipeline = channel.Pipeline;
                    pipeline.AddLast("crypto", new TcpPacketEncryptor(session));
                    pipeline.AddLast("sizer_enc", new TcpPacketSizePrepender(session));
                    pipeline.AddLast("sizer_dec", new TcpPacketSizeDecoder(session));
                    pipeline.AddLast("codec", new TcpPacketCodec(session));
                    pipeline.AddLast("manager", session);

                }))
                .Group(group).LocalAddress(port);
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
