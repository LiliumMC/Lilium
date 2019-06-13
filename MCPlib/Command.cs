using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MCPlib
{
    abstract class Command
    {
        public static bool hasArg(string command)
        {
            int first_space = command.IndexOf(' ');
            return (first_space > 0 && first_space < command.Length - 1);
        }
        public static List<string> getArg(string command)
        {
            Regex regex = new Regex(@"\s+(\S+)");
            MatchCollection matches = regex.Matches(command);
            List<string> args = new List<string>();
            foreach(Match match in matches)
            {
                args.Add(match.Result("$1"));
            }
            return args;
        }

    }
}
