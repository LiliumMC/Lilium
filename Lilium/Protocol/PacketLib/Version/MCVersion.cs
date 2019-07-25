using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Protocol.PacketLib.Version
{
    public class MCVersion
    {
        public virtual void initClientHandshake() { }
        public virtual void initServerHandshake() { }
        public virtual void initClientLogin() { }
        public virtual void initServerLogin() { }
        public virtual void initClientGame() { }
        public virtual void initServerGame() { }
        public virtual void initClientStatus() { }
        public virtual void initServerStatus() { }
    }
}
