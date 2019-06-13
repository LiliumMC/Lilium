using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPlib
{
    public class ServerData
    {
        public static string ServerName = "Lilium Test";
        public static string LobbyServer = "Lobby";
        public static bool CustomMOTD = false;

        public static char vColorChar = '§';

        public static string MsgServerNotFound = "§c找不到这个服务器";

        public static string getColorTag(char colorcode)
        {
            switch (colorcode)
            {
                /* MC 1.7+ Name           MC 1.6 Name           Classic tag */
                case '0':        /*  Blank if same  */      return "black";
                case '1': return "dark_blue";
                case '2': return "dark_green";
                case '3': return "dark_aqua";
                case '4': return "dark_red";
                case '5': return "dark_purple";
                case '6': return "dark_yellow";
                case '7': return "gray";
                case '8': return "dark_gray";
                case '9': return "blue";
                case 'a': return "green";
                case 'b': return "aqua";
                case 'c': return "red";
                case 'd': return "magenta";
                case 'e': return "yellow";
                case 'f': return "white";
                default: return "";
            }
        }
    }
}
