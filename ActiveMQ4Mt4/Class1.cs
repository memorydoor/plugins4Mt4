using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using Apache.NMS.Util;
using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ActiveMQ4Mt4
{
    public class Class1
    {

        public static ISession session;
        public static IConnection connection;

        [DllExport("Connect", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string Connect() {
            string url = "activemq:tcp://localhost:61616";
            try
            {
                Uri connecturi = new Uri(url);

                Console.WriteLine("About to connect to " + connecturi);

                // NOTE: ensure the nmsprovider-activemq.config file exists in the executable folder.

                IConnectionFactory factory = new NMSConnectionFactory(connecturi);

                connection = factory.CreateConnection("admin", "password");
                session = connection.CreateSession();
            }
            catch (Exception e)
            {
                return "ERROR when connect to " + url + ", details:" + e.Message.ToString();
            }
            return "";
        }

        protected static TimeSpan receiveTimeout = TimeSpan.FromSeconds(2);


        [DllExport("GetMessage", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string GetMessage([MarshalAs(UnmanagedType.LPWStr)]string queueName)
        {
            try
            {
                IDestination destination = SessionUtil.GetDestination(session, "queue://" + queueName);
                Console.WriteLine("Using destination: " + destination);

                string result = "";
                // Create a consumer and producer
                using (IMessageConsumer consumer = session.CreateConsumer(destination))
                {
                    // Start the connection so that messages will be processed.
                    connection.Start();
                    // Consume a message
                    ITextMessage message = consumer.Receive(receiveTimeout) as ITextMessage;
                    if (message == null)
                    {
                        Console.WriteLine("No message received!");
                    }
                    else
                    {
                        result = message.Text.ToString();
                        Console.WriteLine("Received message with ID:   " + message.NMSMessageId);
                        Console.WriteLine("Received message with text: " + message.Text);
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                return "ERROR when get message from queue: " + queueName + ", details:" + e.Message.ToString();
            }
            
        }

        [DllExport("SendMessageToQueue", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string SendMessageToQueue(
            [MarshalAs(UnmanagedType.LPWStr)]string queueName,
            [MarshalAs(UnmanagedType.LPWStr)]string message) {
            try
            {
                IDestination destination = SessionUtil.GetDestination(session, "queue://" + queueName);
                Console.WriteLine("Using destination: " + destination);

                // Create a consumer and producer
                using (IMessageProducer producer = session.CreateProducer(destination))
                {
                    // Start the connection so that messages will be processed.
                    connection.Start();

                    // Send a message
                    ITextMessage request = session.CreateTextMessage(message);
                    //request.NMSCorrelationID = "abc";
                    //request.Properties["NMSXGroupID"] = "cheese";
                    //request.Properties["myHeader"] = "Cheddar";

                    producer.Send(request);
                }
            }
            catch (Exception e)
            {
                return "ERROR when send message to queue: " + queueName + ", details:" + e.Message.ToString();
            }

            return "";
        }
    }
}
