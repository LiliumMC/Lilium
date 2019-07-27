using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Protocol.Message
{
    public enum HandleStates:int
    {
        HandShake=0,
        Login=1,
        Game=2,
        Status=3
    }
}
