using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tcp4Mt4
{
    public class Class1
    {

        public static TcpClient tcpclnt;
        private static string hostStr;
        private static string portStr;

        [DllExport("TcpConnect", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string TcpConnect(
             [MarshalAs(UnmanagedType.LPWStr)]string host,
             [MarshalAs(UnmanagedType.LPWStr)]string port)
        {
            hostStr = host;
            portStr = port;
            return internalTcpConnect(host, port);
        }

        private static string internalTcpConnect(string host, string port)
        {
            try
            {
                tcpclnt = new TcpClient();
                tcpclnt.Connect(host, int.Parse(port));
            }
            catch (Exception e)
            {

                return "ERROR:" + e.Message.ToString();
            }

            return "";
        }

        public static Stream stm;

        [DllExport("TcpGet", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string TcpGet(
            [MarshalAs(UnmanagedType.LPWStr)]string request) {
            string result = "";
            try
            {
                stm = tcpclnt.GetStream();
                StreamWriter sw = new StreamWriter(stm);

                sw.WriteLine("a");
                sw.Flush();

                result = new StreamReader(stm).ReadLine();
            }
            catch (Exception e)
            {
                if (!tcpclnt.Connected) {
                    return internalTcpConnect(hostStr, portStr);
                }
                return "ERROR: " + e.Message.ToString();
            }

            return result;
        }
    }
}
