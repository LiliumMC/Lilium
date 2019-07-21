using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net
{
    interface SessionFactory
    {
        public ConnectionListener createServerListener(Server server);
    }
}
