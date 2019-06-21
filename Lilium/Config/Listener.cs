using System.Collections.Generic;

namespace Lilium.Config
{
    public sealed class Listener
    {
        public string Host { set; get; }
        public int Port { set; get; }
        public int MaxPlayers { set; get; }
        public int TabSize { set; get; }
        public bool OnlineMode { set; get; }
        public bool SupportForge { set; get; }
        public bool IPForward { set; get; }
        public string Motd { set; get; }
        public string CustomServerName { set; get; }
        public List<string> Priorities { set; get; }
    }
}
