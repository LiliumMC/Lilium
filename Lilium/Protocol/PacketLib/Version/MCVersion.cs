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

        public const int MC172Version = 4;
        public const int MC1710Version = 5;
        public const int MC18Version = 47;
        public const int MC19Version = 107;
        public const int MC191Version = 108;
        public const int MC110Version = 210;
        public const int MC111Version = 315;
        public const int MC17w13aVersion = 318;
        public const int MC112pre5Version = 332;
        public const int MC17w31aVersion = 336;
        public const int MC17w45aVersion = 343;
        public const int MC17w46aVersion = 345;
        public const int MC17w47aVersion = 346;
        public const int MC18w01aVersion = 352;
        public const int MC18w06aVersion = 357;
        public const int MC113pre4Version = 386;
        public const int MC113pre7Version = 389;
        public const int MC113Version = 393;
        public const int MC114Version = 477;
        public const int MC1142Version = 485;
    }
}
