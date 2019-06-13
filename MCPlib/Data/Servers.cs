using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MCPlib.Data
{
    public class Servers
    {
        public static Dictionary<string, Server> servers = new Dictionary<string, Server>();
        
        public class Server
        {
            public string Host { get; set; }
            public ushort Port { get; set; }
            public int Protocol { get; set; }
        }
    }
}
