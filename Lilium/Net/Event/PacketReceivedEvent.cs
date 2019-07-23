using Lilium.Protocol.PacketLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Net.Event
{
    public class PacketReceivedEvent : ISessionEvent
    {
        private Session session;
        private Packet packet;
        public PacketReceivedEvent(Session session,Packet packet)
        {
            this.session = session;
            this.packet = packet;
        }
        public Session getSession()
        {
            return session;
        }
        public Packet getPacket()
        {
            return packet;
        }
        public void Call(ISessionListener paramListener)
        {
            paramListener.PacketReceived(this);
        }
    }
}
