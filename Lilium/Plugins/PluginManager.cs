using Lilium.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Lilium.Plugins
{
    class PluginManager
    {
        private HandleServer server;
        private List<Plugin> plugins = new List<Plugin>();
        public PluginManager(HandleServer server)
        {
            this.server = server;
        }
        public void LoadPlugins(string folder)
        {
            DirectoryInfo pluginFolder = new DirectoryInfo(folder);
            if (!pluginFolder.Exists)
                pluginFolder.Create();
            foreach (FileInfo pluginFile in pluginFolder.GetFiles())
            {
                Assembly asm = Assembly.LoadFile(pluginFile.FullName);
                Type[] t = asm.GetExportedTypes();
                foreach (Type type in t)
                {
                    if (type.BaseType != null && type.BaseType.Name=="Plugin")
                    {
                        Plugin plugin = (Plugin)Activator.CreateInstance(type);
                        plugins.Add(plugin);
                        plugin.OnLoad();
                        Debug.Log("载入" + plugin, "Plugin");
                    }
                }
            }
        }
        public void EnablePlugins()
        {
            foreach(Plugin plugin in plugins)
            {
                plugin.Init(server);
                plugin.OnEnable();
            }
        }
    }
}
