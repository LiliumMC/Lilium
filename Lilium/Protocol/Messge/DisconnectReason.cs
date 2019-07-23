using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Protocol.Message
{
    public enum DisconnectReason
    {
        InGameKick,
        LoginRejected,
        ConnectionLost,
        ConnectFailed
    }
}
