using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;
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
            // xml configs
            //Logger nlogLogger = InitializeNlog();
            //Serilog.ILogger serilogLogger = InitializeSerilog();
            //nlogLogger.Info("This is an information log");
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine("SERILOG PART");
            //serilogLogger.Information("This is an information log");

            // json configs
            //Logger nlogLogger = InitializeNlog_V2();
            //Serilog.ILogger serilogLogger = InitializeSerilog_V2();

            //nlogLogger.Info("This is an information log");
            //serilogLogger.Information("This is an information log");

            // C# configs
            Logger nlogLogger = InitializeNlog_V3();
            Serilog.ILogger serilogLogger = InitializeSerilog_V3();

            nlogLogger.Info("This is an information log");
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

        private static Serilog.ILogger InitializeSerilog_V2()
        {
            IConfiguration config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .Build();

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();

            // self logging setting
            Serilog.Debugging.SelfLog.Enable(msg => File.AppendAllText("logs\\serilog_internallog.log", msg, Encoding.UTF8));

            return Log.Logger;
        }

        private static Logger InitializeNlog_V2()
        {
            IConfiguration config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();

            LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));

            Logger nlogLogger = NLog.Web.NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();

            return nlogLogger;
        }

        private static Serilog.ILogger InitializeSerilog_V3()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .WriteTo.File("logs\\Serilog..log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // self logging setting
            Serilog.Debugging.SelfLog.Enable(msg => File.AppendAllText("logs\\serilog_internallog.log", msg, Encoding.UTF8));

            return Log.Logger;
        }

        private static Logger InitializeNlog_V3()
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "${basedir}/logs/Nlog.${shortdate}.log", Layout = "${longdate} ${logger} ${uppercase:${level}} ${message}", Encoding = Encoding.UTF8 };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole") { Layout = "${longdate} ${logger} ${uppercase:${level}} ${message}" };

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;

            Logger nlogLogger = NLog.Web.NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();

            return nlogLogger;
        }
    }
}
