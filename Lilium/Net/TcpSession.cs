using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Lilium.Protocol.PacketLib;
using Lilium.Protocol;
using Lilium.Net.Handlers;
using Lilium.Protocol.Message;
using Lilium.Net.Event;
using System.Threading;

namespace Lilium.Net
{
    abstract class TcpSession:SimpleChannelInboundHandler<Packet>,Session
    {
        private string Host { get; set; }
        private int Port { get; set; }
        private PacketProtocol packetProtocol { get; set; }
        private List<ISessionListener> listeners = new List<ISessionListener>();
        protected IChannel channel;
        public int ProtocolVersion { get; set; }
        public int CompressionTreshold { get; set; } = 0;
        private List<Packet> packets = new List<Packet>();
        private AutoResetEvent packetHandleEvent = new AutoResetEvent(false);
        private Thread packetHandleThread;

        public bool Connected { get
            {
                return this.channel != null && this.channel.Open && !this.disconnected;
            } }

        protected bool disconnected = false;

        public TcpSession(string host,int port,PacketProtocol type)
        {
            Host = host;
            Port = port;
            packetProtocol = type;
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
        public PacketProtocol getPacketProtocol()
        {
            return packetProtocol;
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
                ctx.Channel.CloseAsync().Wait();
                return;
            }
            this.channel = ctx.Channel;
            this.packetHandleThread = new Thread(new ThreadStart(() =>
              {
                  try
                  {
                      while (!disconnected)
                      {
                          packetHandleEvent.WaitOne();
                          Packet packet = packets[0];
                          packets.RemoveAt(0);
                          CallEvent(new PacketReceivedEvent(this, packet));
                      }
                  }
                  catch (Exception e)
                  {
                      ExceptionCaught(null, e);
                  }
              }));
            this.packetHandleThread.IsBackground = true;
            this.packetHandleThread.Start();
            CallEvent(new ConnectedEvent(this));
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            if (this.channel == context.Channel)
            {
                this.Disconnect(DisconnectReason.ConnectionLost,"Connection Lost.");
            }
        }
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            DisconnectReason reason = DisconnectReason.ConnectFailed;
            string message = exception.Message;
            Disconnect(reason,message);
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, Packet msg)
        {
            packets.Add(msg);
            packetHandleEvent.Set();
        }

        public void Connect()
        {
        }

        public void Disconnect(DisconnectReason reason, string message)
        {
            if (this.disconnected)
                return;

            this.disconnected = true;
            if (this.packetHandleThread != null)
            {
                this.packetHandleThread.Abort();
                this.packetHandleThread = null;
            }
        }
    }
}
