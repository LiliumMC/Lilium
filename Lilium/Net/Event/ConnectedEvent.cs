using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.Event
{
    class ConnectedEvent:ISessionEvent
    {
        private Session session;
        public ConnectedEvent(Session session)
        {
            this.session = session;
        }
        public Session getSession()
        {
            return session;
        }
        public void Call(ISessionListener paramListener)
        {
            paramListener.Connected(this);
        }
    }
}
