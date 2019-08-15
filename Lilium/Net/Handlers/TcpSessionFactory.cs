using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.Handlers
{
    public class TcpSessionFactory : ISessionFactory
    {
        public Session createClientSession(HandleClient client)
        {
            return new TcpClientSession(client.Host, client.Port, client.getPacketProtocol(), client);
        }

        public ConnectionListener createServerListener(HandleServer server)
        {
            return new TcpConnectionListener(server.Host, server.Port, server);
        }
    }
}
