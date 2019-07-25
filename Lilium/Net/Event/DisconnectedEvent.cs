using Lilium.Protocol.Message;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.Event
{
    public class DisconnectedEvent : ISessionEvent
    {
        private Session session;
        private DisconnectReason reason;
        private string message;
        public DisconnectedEvent(Session session, DisconnectReason reason, string message)
        {
            this.session = session;
            this.reason = reason;
            this.message = message;
        }
        public Session getSession()
        {
            return this.session;
        }
        public DisconnectReason getReason()
        {
            return this.reason;
        }
        public string getMessage()
        {
            return this.message;
        }
        public void Call(ISessionListener paramListener)
        {
            paramListener.Disconnected(this);
        }
    }
}
