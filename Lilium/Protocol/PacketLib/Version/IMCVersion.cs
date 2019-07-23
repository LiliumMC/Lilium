using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Protocol.PacketLib.Version
{
    public interface IMCVersion
    {
        void initClientHandshake();
        void initServerHandshake();
        void initClientLogin();
        void initServerLogin();
        void initClientGame();
        void initServerGame();
        void initClientStatus();
        void initServerStatus();
    }
}
