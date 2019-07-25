using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.Event
{
    public interface ISessionListener
    {
        void Connected(ConnectedEvent paramEvent);
        void PacketReceived(PacketReceivedEvent paramEvent);
        void Disconnecting(DisconnectingEvent paramEvent);
        void Disconnected(DisconnectedEvent paramEvent);
    }
}
