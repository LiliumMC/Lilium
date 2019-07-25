using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.IO;
using YamlDotNet.Serialization;
using Lilium.Config;
using Lilium.Net;
using Lilium.Protocol;
using Lilium.Net.Handlers;
using Lilium.Protocol.PacketLib.Version;

namespace Lilium
{
    class Program
    {
        internal static ILoggerRepository repository = null;
        internal static ILog log = null;
        internal static YamlConfig config = null;

        static MCLilium listener;
        static void Main(string[] args)
        {
            Debug.Log("加载配置ing");
            repository = LogManager.CreateRepository("Lilium");
            XmlConfigurator.ConfigureAndWatch(repository, new FileInfo("log4net.config"));
            log = LogManager.GetLogger(repository.Name, "Lilium");

            try
            {
                using (var reader = new StreamReader(@"config.yml"))
                {
                    config = new Deserializer().Deserialize<YamlConfig>(reader.ReadToEnd());
                }
            }
            catch (FileNotFoundException)
            {
                config = new YamlConfig();
                var yaml = new Serializer().Serialize(config);
                File.WriteAllText(@"config.yml", yaml);
            }
            catch (Exception)
            {
                throw;
            }
            StartListening();
        }

        static void StartListening()
        {
            listener = new MCLilium();
            listener.Start();
        }
    }
}
