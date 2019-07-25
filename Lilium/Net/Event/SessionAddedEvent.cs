using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.Event
{
    public class SessionAddedEvent:ServerEvent
    {
        private Session session;
        public SessionAddedEvent(Session session)
        {
            this.session = session;
        }

        public void Call(IServerListener listener)
        {
            listener.SessionAdded(this);
        }

        public Session getSession()
        {
            return this.session;
        }
    }
}
