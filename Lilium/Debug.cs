using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium
{
    class Debug
    {
        public static void Log(object text,string title=null)
        {
            ConsoleIO.WriteLine(title == null ? text : "[" + title + "] " + text);
        }
    }
}
