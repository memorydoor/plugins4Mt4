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
    }
}
