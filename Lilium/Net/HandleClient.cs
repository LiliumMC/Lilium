using Lilium.Protocol.PacketLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net
{
    public class HandleClient
    {
        public string Host { get; private set; }
        public int Port { get; private set; }
        private PacketProtocol protocol;
        private Session session;
        public HandleClient(string host, int port, PacketProtocol type, ISessionFactory factory)
        {
            this.Host = host;
            this.Port = port;
            this.protocol = type;
            this.session = factory.createClientSession(this);
        }
        public PacketProtocol getPacketProtocol()
        {
            return this.protocol;
        }
        public Session getSession()
        {
            return this.session;
        }
    }
}
