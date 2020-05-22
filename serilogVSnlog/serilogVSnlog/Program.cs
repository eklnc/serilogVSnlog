using NLog;
using Serilog;
using System;
using System.IO;
using System.Text;

namespace serilogVSnlog
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger nlogLogger = InitializeNlog();
            Serilog.ILogger serilogLogger = InitializeSerilog();

            nlogLogger.Info("This is an information log");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("SERILOG PART");
            serilogLogger.Information("This is an information log");

            Console.ReadKey();
        }

        private static Serilog.ILogger InitializeSerilog()
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();

            // self logging setting
            Serilog.Debugging.SelfLog.Enable(msg => File.AppendAllText("logs\\serilog_internallog.log", msg, Encoding.UTF8));

            return Log.Logger;
        }

        private static Logger InitializeNlog()
        {
            Logger nlogLogger = LogManager.GetCurrentClassLogger();

            // your custom config name
            // Logger nlogLogger = NLog.Web.NLogBuilder.ConfigureNLog("YOUR_CONFIG_NAME.config").GetCurrentClassLogger();

            return nlogLogger;
        }
    }
}
