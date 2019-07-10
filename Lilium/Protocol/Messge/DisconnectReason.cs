using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Protocol.Message
{
    enum DisconnectReason
    {
        InGameKick,
        LoginRejected,
        ConnectionLost,
        ConnectFailed
    }
}
