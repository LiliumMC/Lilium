using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPlib.Protocol
{
    interface IMinecraftCo
    {
        string getServerHost();
        ushort getServerPort();
        void receivePacket(int packetID, List<byte> packetData);
        void OnConnectionLost(Conn.DisconnectReason reason, string message);
        void OnLogin();
    }
}
