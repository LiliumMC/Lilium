using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net
{
    interface SessionFactory
    {
        ConnectionListener createServerListener(HandleServer server);
    }
}
