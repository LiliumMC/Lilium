using Lilium.Net.Event;
using Lilium.Protocol.Message;
using Lilium.Protocol.PacketLib.Packets.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Protocol.Handlers
{
    class ClientListener: SessionAdapter
    {
        public override void Connected(ConnectedEvent Event)
        {
            MinecraftProtocol protocol = (MinecraftProtocol)Event.getSession().getPacketProtocol();
            protocol.setProtocolState(HandleStates.HandShake, true);
            if (protocol.States == HandleStates.Login)
            {
                Event.getSession().Send(new HandshakePacket(protocol.Protocol, Event.getSession().getHost(), Event.getSession().getPort(), 0x02));
                protocol.setProtocolState(HandleStates.Login, true);
                Event.getSession().Send(new LoginPacket(Event.getSession().getFlag(MinecraftConstants.NAME_KEY).ToString()));
            }else if (protocol.States == HandleStates.Status)
            {
                Event.getSession().Send(new HandshakePacket(protocol.Protocol, Event.getSession().getHost(), Event.getSession().getPort(), 0x01));
                protocol.setProtocolState(HandleStates.Status, true);
                Event.getSession().Send(new StatusQueryPacket());
            }
        }
    }
}
