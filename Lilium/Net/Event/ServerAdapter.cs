using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.Event
{
    public class ServerAdapter : IServerListener
    {
        public virtual void SessionAdded(SessionAddedEvent paramEvent)
        {
        }

        public virtual void SessionRemoved(SessionRemovedEvent paramEvent)
        {
        }
    }
}
