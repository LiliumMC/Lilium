using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.Handlers
{
    class TcpSessionFactory : SessionFactory
    {
        public ConnectionListener createServerListener(HandleServer server)
        {
            return new TcpConnectionListener(server.Host, server.Port, server);
        }
    }
}
