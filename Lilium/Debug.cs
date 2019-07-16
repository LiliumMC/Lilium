using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium
{
    class Debug
    {
        public static void Log(string text,string title=null)
        {
            Console.WriteLine(title == null ? text : "[" + title + "] " + text);
        }
    }
}
