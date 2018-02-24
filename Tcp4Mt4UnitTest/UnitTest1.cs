using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tcp4Mt4;

namespace Tcp4Mt4UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Class1.TcpConnect("127.0.0.1", "11111");

            string result = Class1.TcpGet("hello");
            Assert.AreEqual("a", result);
        }

        [TestMethod]
        public void TestMethod2()
        {
            for (int i = 0; i < 5; i++) {
                string result = Class1.readClientTcpRequest("11112");

                if (result.StartsWith("request")) {
                    Class1.writeClientTcpResponse("hello" + i);
                }

                Console.Write(result);
            }
            
            Assert.AreEqual("a", "a");
        }

        [TestMethod]
        public void TestMethod3()
        {
            
            string result = Class1.TcpClose();
            Assert.AreEqual("a", result);
        }
    }
}
