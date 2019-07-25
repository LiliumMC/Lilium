using Lilium.Config;
using Lilium.Net;
using Lilium.Net.Handlers;
using Lilium.Plugins;
using Lilium.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium
{
    class MCLilium
    {
        HandleServer listener;
        PluginManager pluginManager;
        public MCLilium()
        {
            listener = new HandleServer(Program.config.Listener.Host, Program.config.Listener.Port, new MinecraftProtocol(5), new TcpSessionFactory());
            pluginManager = new PluginManager(listener);
        }
        public void Start()
        {
            Debug.Log("读取插件中");
            pluginManager.LoadPlugins("plugins");
            pluginManager.EnablePlugins();
            StartListening();
        }
        public void StartListening()
        {

            listener.Bind().Wait();
            Debug.Log(string.Format("开始监听:{0}:{1}", Program.config.Listener.Host, Program.config.Listener.Port));
            while (listener.isListening)
            {
                string input = ConsoleIO.ReadLine();
                if (input != null)
                {

                }
            }
        }
    }
}
