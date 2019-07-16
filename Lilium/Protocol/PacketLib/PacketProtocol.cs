using Lilium.Crypto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Protocol.PacketLib
{
    abstract class PacketProtocol
    {
        private Dictionary<int, Type> incoming = new Dictionary<int, Type>();
        private Dictionary<Type, int> outgoing = new Dictionary<Type, int>();

        public abstract CryptoHandler getCrypto();
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
    }
}
