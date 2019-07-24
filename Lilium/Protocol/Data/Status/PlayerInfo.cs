using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Protocol.Data.Status
{
    public class PlayerInfo
    {
        public int Max { get; set; }
        public int Online { get; set; }
        public PlayerInfo(int max,int online)
        {
            this.Max = max;
            this.Online = online;
        }
        public override string ToString()
        {
            return string.Format("{{\"max\":{0},\"online\":{1}}}", this.Max, this.Online);
        }
    }
}
