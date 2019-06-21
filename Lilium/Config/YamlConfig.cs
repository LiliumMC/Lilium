using System;
using System.Collections.Generic;

namespace Lilium.Config
{
    public sealed class YamlConfig
    {
        public string ServerID { set; get; } = Guid.NewGuid().ToString();

        public Listener Listener { set; get; } = new Listener()
        {
            Host = "0.0.0.0", 
            Port = 25565, 
            MaxPlayers = 2333, 
            TabSize = 60, 
            OnlineMode = true, 
            SupportForge = true,
            IPForward = true, 
            Motd = "Another bungee implement server.", 
            CustomServerName = "Lilium", 
            Priorities = new List<string>()
            {
                "lobby", 
                "pvp"
            }
        };

        public List<Server> Servers { set; get; } = new List<Server>()
        {
            new Server()
            {
                Name = "lobby",
                Address = "127.0.0.1",
                Port = 25570,
                Restricted = false
            },
            new Server()
            {
                Name = "pvp",
                Address = "127.0.0.1",
                Port = 25571,
                Restricted = true
            }
        };
    }
}
