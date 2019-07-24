using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Protocol.Data.Status
{
    public class VersionInfo
    {
        public string Name { get; set; }
        public int Protocol { get; set; }
        public VersionInfo(string name,int protocol)
        {
            this.Name = name;
            this.Protocol = protocol;
        }
        public override string ToString()
        {
            return string.Format("{{\"name\":\"{0}\",\"protocol\":{1}}}", this.Name, this.Protocol);
        }
    }
}
