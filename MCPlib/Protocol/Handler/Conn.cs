using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPlib.Protocol
{
    abstract class Conn
    {
        public enum DisconnectReason { InGameKick, LoginRejected, ConnectionLost };
    }
}
