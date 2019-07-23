using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.Event
{
    public interface ISessionEvent
    {
        void Call(ISessionListener paramListener);
    }
}
