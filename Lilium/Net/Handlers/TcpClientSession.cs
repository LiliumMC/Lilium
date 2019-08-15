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
    class TcpClientSession:TcpSession
    {
        private HandleClient client;
        private IEventLoopGroup group;
        public TcpClientSession(string host, int port, PacketProtocol type,HandleClient client) : base(host, port, type)
        {
            this.client = client;
        }
        public override void Connect(bool wait)
        {
            if (wait)
                Connect().Wait();
            else
                Connect();
        }
        public async Task Connect()
        {
            try
            {
                Bootstrap bootstrap = new Bootstrap();
                group = new MultithreadEventLoopGroup();
                bootstrap
                .Channel<TcpSocketChannel>()
                .Group(group)
                .Handler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    getPacketProtocol().newClientSession(this);
                    channel.Configuration.SetOption(ChannelOption.IpTos, 24);
                    channel.Configuration.SetOption(ChannelOption.TcpNodelay, false);
                    IChannelPipeline pipeline = channel.Pipeline;

                    pipeline.AddLast("crypto", new TcpPacketEncryptor(this));
                    pipeline.AddLast("sizer_enc", new TcpPacketSizePrepender(this));
                    pipeline.AddLast("sizer_dec", new TcpPacketSizeDecoder(this));
                    pipeline.AddLast("codec", new TcpPacketCodec(this));
                    pipeline.AddLast("manager", this);

                }));
                Debug.Log(this.getHost() + ":" + this.getPort());
                await bootstrap.ConnectAsync(IPAddress.Parse(this.getHost()), this.getPort());
                Debug.Log(this.getHost() + ":" + this.getPort());
            }
            catch(Exception e)
            {
                ExceptionCaught(null, e);
            }
        }
    }
}
