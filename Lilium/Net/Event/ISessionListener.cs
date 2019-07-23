using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.Event
{
    interface ISessionListener
    {
        void Connected(ConnectedEvent paramEvent);
        void PacketReceived(PacketReceivedEvent paramEvent);
    }
}
