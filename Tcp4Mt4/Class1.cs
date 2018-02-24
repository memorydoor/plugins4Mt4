using RGiesecke.DllExport;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Tcp4Mt4
{
    public class Class1
    {
        /* Start: tcp client */

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


        [DllExport("TcpClose", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string TcpClose()
        {
            try
            {
                if (stm != null) {
                    stm.Close();
                }

                if (tcpclnt != null)
                {
                    tcpclnt.Close();
                }

            }
            catch (Exception e)
            {
                return "ERROR: " + e.Message.ToString();
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

                sw.WriteLine(request);
                sw.Flush();

                result = new StreamReader(stm).ReadLine();

                stm.Close();
                stm = null;
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

        /* End: tcp client */

        /* Start: tcp server */

        static TcpListener server;
        static TcpClient client;
        static NetworkStream stream;
        static StreamReader sr;

        [DllExport("readClientTcpRequest", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string readClientTcpRequest([MarshalAs(UnmanagedType.LPWStr)]string portStr)
        {
            String data = "";
            try
            {
                if (server == null)
                {
                    // Set the TcpListener on port 13000.
                    Int32 port = int.Parse(portStr);
                    IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                    // TcpListener server = new TcpListener(port);
                    server = new TcpListener(localAddr, port);

                    // Start listening for client requests.
                    server.Start();
                    data = "Listening on: " + port + ", " + server.Server.Connected;
                    Console.Write(data);
                    return data;
                }

                Console.Write(data);
                // Perform a blocking call to accept requests.
                // You could also user server.AcceptSocket() here.
                //Socket socket = server.AcceptSocket();
                if (client == null)
                {

                    client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    IPEndPoint endPoint = (IPEndPoint)client.Client.RemoteEndPoint;
                    // .. or LocalEndPoint - depending on which end you want to identify

                    IPAddress ipAddress = endPoint.Address;

                    // get the port
                    int clientPort = endPoint.Port;

                    data = "Connected from " + ipAddress + ":" + clientPort;
                    return data;
                }

                if (client.Connected == false)
                {
                    client.Close();
                }

                // Buffer for reading data
                Byte[] bytes = new Byte[256];

                // Get a stream object for reading and writing
                if (stream == null)
                {
                    stream = client.GetStream();
                    sr = new StreamReader(stream);
                }


                //StreamWriter sw = new StreamWriter(ns);

                data = sr.ReadLine();
                Console.WriteLine("Received: {0}", data);
                data = "request:" + data;

            }
            catch (Exception e)
            {
                Console.WriteLine("SocketException: {0}", e);
                data = e.Message;

                try
                {

                    server.Stop();
                    server = null;
                    client = null;
                    stream = null;
                    sr = null;
                    sw = null;
                }
                catch (Exception e1)
                {
                    Console.WriteLine("SocketException: {0}", e1);
                    data = data + " " + e1.Message;
                }
            }
            return data;
        }

        static StreamWriter sw;

        [DllExport("writeClientTcpResponse", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string writeClientTcpResponse([MarshalAs(UnmanagedType.LPWStr)]string response)
        {
            String data = "";
            try
            {
                if (sw == null) {
                    sw = new StreamWriter(stream);
                }

                sw.WriteLine(response);
                sw.Flush();
                Console.WriteLine("Sent: {0}", response);
                data = "Sent:" + response;
            }
            catch (Exception e)
            {
                Console.WriteLine("SocketException: {0}", e);

                data = e.Message;

                try
                {
                    server.Stop();
                    server = null;
                    client = null;
                    stream = null;
                    sr = null;
                    sw = null;
                }
                catch (Exception e1)
                {
                    Console.WriteLine("SocketException: {0}", e1);
                    data = data + " " + e1.Message;
                }
            }

            return data;
        }

        [DllExport("stopTcpServer", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string stopTcpServer() {
            string data = "";
            try {
                if (server != null)
                {
                    server.Stop();
                }

                server = null;
                client = null;
                stream = null;
                sr = null;
                sw = null;
            }
            catch (Exception e)
            {
                Console.WriteLine("SocketException: {0}", e);

                data = e.Message;
            }
            

            return data;
        }
        /* End: tcp server */
    }
}
