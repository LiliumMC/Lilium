using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.Handlers
{
    class TcpSessionFactory : ISessionFactory
    {
        public Session createClientSession(HandleClient client)
        {
            throw new NotImplementedException();
        }

        public ConnectionListener createServerListener(HandleServer server)
        {
            return new TcpConnectionListener(server.Host, server.Port, server);
        }
    }
}
