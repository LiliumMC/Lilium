using Lilium.Net.Event;
using Lilium.Protocol.Message;
using Lilium.Protocol.PacketLib;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Lilium.Net
{
    interface Session
    {
        string getHost();
        int getPort();
        PacketProtocol getPacketProtocol();
        int ProtocolVersion { get; set; }
        int CompressionTreshold { get; set; }
        bool Connected { get; }
        Task Send(Packet pcket);
        void Connect();
        void Disconnect(DisconnectReason reason, string message);
        void AddListener(ISessionListener paramListener);
        void RemoveListener(ISessionListener paramListener);
        void CallEvent(ISessionEvent paramEvent);
    }
}
