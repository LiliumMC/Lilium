using Lilium.Net.Event;
using Lilium.Protocol.Messge;
using Lilium.Protocol.PacketLib.Packets.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Protocol.Handlers
{
    class ServerListener : ISessionListener
    {
        public void Connected(ConnectedEvent paramEvent)
        {
            //throw new NotImplementedException();
        }

        public void PacketReceived(PacketReceivedEvent Event)
        {
            MinecraftProtocol protocol = (MinecraftProtocol)Event.getSession().getPacketProtocol();
            switch (protocol.States)
            {
                case HandleStates.HandShake:
                    HandshakePacket packet = (HandshakePacket)Event.getPacket();
                    Console.WriteLine(packet.Host);
                    break;
            }
        }
    }
}
