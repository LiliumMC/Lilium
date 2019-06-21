using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.IO;
using YamlDotNet.Serialization;
using Lilium.Config;

namespace Lilium
{
    class Program
    {
        internal static ILoggerRepository repository = null;
        internal static ILog log = null;
        internal static YamlConfig config = null;

        static void Main(string[] args)
        {
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

        }
    }
}
