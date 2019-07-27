using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net
{
    public interface ISessionFactory
    {
        ConnectionListener createServerListener(HandleServer server);
        Session createClientSession(HandleClient client);
    }
}
