using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.Event
{
    public interface ServerEvent
    {
        void Call(IServerListener listener);
    }
}
