using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using Lilium.Protocol.PacketLib;
using Lilium.Protocol;
using Lilium.Net.Handlers;

namespace Lilium.Net
{
    abstract class TcpSession:SimpleChannelInboundHandler<Packet>,Session
    {
        private string Host { get; set; }
        private int Port { get; set; }
        private PacketType packetType { get; set; }
        private List<ISessionListener> listeners = new List<ISessionListener>();
        protected IChannel channel;
        public int ProtocolVersion { get; set; }
        public int CompressionTreshold { get; set; } = 0;
        protected bool disconnected = false;

        public TcpSession(string host,int port,PacketType type)
        {
            Host = host;
            Port = port;
            packetType = type;
            ProtocolVersion = type.Protocol;
        }
        public string getHost()
        {
            return Host;
        }
        public int getPort()
        {
            return Port;
        }
        public PacketType getPacketType()
        {
            return packetType;
        }
        public async Task Send(Packet packet)
        {
            if (channel == null)
                return;
            try
            {
                await channel.WriteAndFlushAsync(packet);
            }catch(Exception e)
            {
                ExceptionCaught(null, e);
            }
        }
        public void setCompressionThreshold(int threshold)
        {
            CompressionTreshold = threshold;
            if (channel != null)
            {
                if (ProtocolVersion >= (int)MCVersion.MC18Version && CompressionTreshold > 0)
                {
                    if (channel.Pipeline.Get("compression") == null)
                        channel.Pipeline.AddBefore("codec", "compression", new TcpPacketCompression(this));
                }
                else if (channel.Pipeline.Get("compression") != null)
                    channel.Pipeline.Remove("compression");
            }
        }
        public abstract Task Connect();
        public abstract void Disconnect(DisconnectReason reason, string message);
        public void AddListener(ISessionListener paramListener)
        {
            listeners.Add(paramListener);
        }

        public void RemoveListener(ISessionListener paramListener)
        {
            listeners.Remove(paramListener);
        }
        public void CallEvent(ISessionEvent paramEvent)
        {
            foreach(ISessionListener listener in listeners)
            {
                paramEvent.Call(listener);
            }
        }
        public override void ChannelActive(IChannelHandlerContext ctx)
        {
            if(disconnected || channel != null)
            {
                ctx.Channel.CloseAsync();
                return;
            }
            this.channel = ctx.Channel;
            CallEvent(new ConnectedEvent(this));
        }
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            DisconnectReason reason = DisconnectReason.ConnectFailed;
            string message = exception.Message;
            Disconnect(reason,message);
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, Packet msg)
        {
            
        }
    }
}
