using Lilium.Crypto;
using Lilium.Net;
using Lilium.Protocol.PacketLib.Packets.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Protocol.PacketLib
{
    public abstract class PacketProtocol
    {
        public int Protocol { get; set; }
        private Dictionary<int, Type> incoming = new Dictionary<int, Type>();
        private Dictionary<Type, int> outgoing = new Dictionary<Type, int>();

        public abstract CryptoHandler getCrypto();
        public abstract void newServerSession(Session session);
        public void ClearPackets()
        {
            this.incoming.Clear();
            this.outgoing.Clear();
        }
        public void RegisterIncoming<T>(int id) where T : Packet
        {
            this.incoming.Add(id,typeof(T));
        }
        public void RegisterOutgoing<T>(int id) where T : Packet
        {
            outgoing.Add(typeof(T), id);
        }
        public Packet createIncomingPacket(int id)
        {
            if (incoming.ContainsKey(id))
            {
                return (Packet)Activator.CreateInstance(incoming[id]);
            }
            return new UnknownPacket();
        }
        public int getOutgoingID(Packet packet)
        {
            if (outgoing.ContainsKey(packet.GetType()))
            {
                return outgoing[packet.GetType()];
            }
            throw new Exception("unregistered packet.");
        }
    }
}
