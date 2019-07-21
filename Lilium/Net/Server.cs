using Lilium.Protocol.PacketLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net
{
    class Server
    {
        public string Host { get; set; }
        public int Port { get; set; }
        private PacketProtocol protocol;
        private SessionFactory factory;
        public Server(string host,int port,PacketProtocol protocol,SessionFactory factory)
        {
            this.Host = host;
            this.Port = port;
            this.protocol = protocol;
            this.factory = factory;
        }
        public PacketProtocol createPacketProtocol()
        {
            return this.protocol;
        }
    }
}
