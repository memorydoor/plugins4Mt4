using Newtonsoft.Json.Linq;
using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using log4net;
using log4net.Repository.Hierarchy;
using log4net.Layout;
using log4net.Appender;
using log4net.Core;

namespace json4Mt4
{
    public class Class1
    {

        static ILog log;

        static Class1()
        {
            Setup();
            log4net.Config.BasicConfigurator.Configure();
            log = log4net.LogManager.GetLogger(typeof(Class1));
        }

        public static void Setup()
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
            patternLayout.ActivateOptions();

            RollingFileAppender roller = new RollingFileAppender();
            roller.AppendToFile = true;

            roller.File = @"D:/Log.txt";
            roller.Layout = patternLayout;
            roller.MaxSizeRollBackups = 5;
            roller.MaximumFileSize = "1GB";
            roller.RollingStyle = RollingFileAppender.RollingMode.Size;
            roller.StaticLogFileName = true;
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            MemoryAppender memory = new MemoryAppender();
            memory.ActivateOptions();
            hierarchy.Root.AddAppender(memory);

            hierarchy.Root.Level = Level.Info;
            hierarchy.Configured = true;
        }


        [DllExport("jsonCount", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string jsonCount(
            [MarshalAs(UnmanagedType.LPWStr)]string json,
            [MarshalAs(UnmanagedType.LPWStr)]string path) {
            try
            {
                JObject o = JObject.Parse(json);
                log.Info(path);
                IEnumerable<JToken> acme = o.SelectTokens(path);

                long count = acme.LongCount();
                
                log.Info(count);
                return count.ToString();
            }
            catch (Exception e)
            {
                log.Info(e);

                ShowDLLException(e);    
                return "exception happenned";
            }
        }

        private static void ShowDLLException(Exception e)
        {
            MessageBox.Show(e.Message, "DLL exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }


        [DllExport("getSimpleJson", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string getSimpleJson(
            [MarshalAs(UnmanagedType.LPWStr)]string json,
            [MarshalAs(UnmanagedType.LPWStr)]string path,
            [MarshalAs(UnmanagedType.LPWStr)]string index)
        {
            try
            {
                JObject o = JObject.Parse(json);
                log.Info("path:" + path);
                log.Info("index:" + index);
                IEnumerable<JToken> acme = o.SelectTokens(path);
                string result = acme.ToList()[Int32.Parse(index)].ToString();
                log.Info(result);

                return result;
            }
            catch (Exception e)
            {
                log.Info(e);

                ShowDLLException(e);    
                return "exception happenned";
            }
        }

        [DllExport("getString", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string getString(
            [MarshalAs(UnmanagedType.LPWStr)]string json,
            [MarshalAs(UnmanagedType.LPWStr)]string path)
        {
            JObject o = JObject.Parse(json);
            JToken acme = o.SelectToken(path);
            string result =  acme.ToString();

            return "" + result;
        }

        [DllExport("getInt", CallingConvention = CallingConvention.StdCall)]
        public static int getInt(
            [MarshalAs(UnmanagedType.LPWStr)]string json,
            [MarshalAs(UnmanagedType.LPWStr)]string path)
        {
            JObject o = JObject.Parse(json);
            JToken acme = o.SelectToken(path);
            int result = Int32.Parse(acme.ToString());
            return result;
        }

    }
}
