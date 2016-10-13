using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using RGiesecke.DllExport;
using System.Runtime.InteropServices;

namespace log4Mt4
{
    public class Class1
    {

        public static void Setup()
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
            patternLayout.ActivateOptions();

            RollingFileAppender roller = new RollingFileAppender();
            roller.AppendToFile = true;

            roller.File = @"C:/Log.txt";
            roller.Layout = patternLayout;
            roller.MaxSizeRollBackups = 5;
            roller.MaximumFileSize = "1GB";
            roller.RollingStyle = RollingFileAppender.RollingMode.Date;
            roller.StaticLogFileName = true;
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            MemoryAppender memory = new MemoryAppender();
            memory.ActivateOptions();
            hierarchy.Root.AddAppender(memory);

            hierarchy.Root.Level = Level.Info;
            hierarchy.Configured = true;
        }

        static ILog log;
        static bool writeLogs = false;

        [DllExport("initLog4Mt4", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static void initLog4Mt4(
            [MarshalAs(UnmanagedType.LPWStr)]string appName, 
            [MarshalAs(UnmanagedType.LPWStr)]string ifLog) {

            if ("TRUE" == ifLog || "true" == ifLog) {
                Setup();
                log4net.Config.BasicConfigurator.Configure();
                log = LogManager.GetLogger(appName);

                writeLogs = true;
            }
        }

        [DllExport("info", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static void info([MarshalAs(UnmanagedType.LPWStr)] string message) {
            if (writeLogs) {
                log.Info(message);
            }
        }
    }
}
