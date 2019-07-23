using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.Event
{
    interface ISessionEvent
    {
        void Call(ISessionListener paramListener);
    }
}
