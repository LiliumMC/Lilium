using Lilium.Net.Event;
using Lilium.Protocol.Data.Status;
using Lilium.Protocol.Messge;
using Lilium.Protocol.PacketLib.Packets.Client;
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

        public void Disconnected(DisconnectedEvent paramEvent)
        {
        }

        public void Disconnecting(DisconnectingEvent paramEvent)
        {
        }

        public void PacketReceived(PacketReceivedEvent Event)
        {
            MinecraftProtocol protocol = (MinecraftProtocol)Event.getSession().getPacketProtocol();
            switch (protocol.States)
            {
                case HandleStates.HandShake:
                    if (Event.getPacket().GetType() == typeof(HandshakePacket))
                    {
                        Debug.Log("已连接", Event.getSession().getRemoteAddress().Address.ToString());
                        HandshakePacket packet = (HandshakePacket)Event.getPacket();

                        switch (packet.Intent)
                        {
                            case 0x01:
                                protocol.setProtocolState(HandleStates.Status, false);
                                break;
                            case 0x02:
                                protocol.setProtocolState(HandleStates.Login, false);
                                break;
                            default:
                                throw new InvalidOperationException("Invalid client intent: " + packet.Intent);
                        }
                    }
                    break;
                case HandleStates.Status:
                    if (Event.getPacket().GetType() == typeof(StatusQueryPacket))
                    {
                        StatusInfo info = new StatusInfo();
                        info.Add("description", "\""+Program.config.Listener.Motd+"\"");
                        info.Add("players", new PlayerInfo(Program.config.Listener.MaxPlayers, 0));
                        info.Add("version", new VersionInfo(Program.config.Listener.CustomServerName, 5));
                        Event.getSession().Send(new StatusResponsePacket(info));
                    }else if(Event.getPacket().GetType() == typeof(StatusPingPacket))
                    {
                        Event.getSession().Send(new StatusPongPacket(((StatusPingPacket)Event.getPacket()).Time));
                    }
                    break;
            }
        }
    }
}
