using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.Event
{
    public class SessionRemovedEvent:ServerEvent 
    {
        private Session session;
        public SessionRemovedEvent(Session session)
        {
            this.session = session;
        }

        public void Call(IServerListener listener)
        {
            listener.SessionRemoved(this);
        }

        public Session getSession()
        {
            return this.session;
        }
    }
}
