using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.Event
{
    public class SessionAdapter : ISessionListener
    {
        public virtual void Connected(ConnectedEvent paramEvent)
        {
        }

        public virtual void Disconnected(DisconnectedEvent paramEvent)
        {
        }

        public virtual void Disconnecting(DisconnectingEvent paramEvent)
        {
        }

        public virtual void PacketReceived(PacketReceivedEvent paramEvent)
        {
        }
    }
}
