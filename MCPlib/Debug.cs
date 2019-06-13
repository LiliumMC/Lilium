using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPlib
{
    class Debug
    {
        public static void Log(object msg,string title = null)
        {
            if (title == null)
                Console.WriteLine(msg);
            else
                Console.WriteLine("[" + title + "] " + msg);
        }
    }
}
