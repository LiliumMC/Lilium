using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.IO;

namespace Lilium
{
    class Program
    {
        internal static ILoggerRepository repository = null;
        internal static ILog log = null;

        static void Main(string[] args)
        {
            repository = LogManager.CreateRepository("Lilium");
            XmlConfigurator.ConfigureAndWatch(repository, new FileInfo("log4net.config"));
            log = LogManager.GetLogger(repository.Name, "Lilium");

        }
    }
}
