using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ActiveMQ4Mt4;

namespace ActiveMQ4Mt4UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string result = Class1.Connect();
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void TestMethod2()
        {
            string result = Class1.Connect();
            string result1 = Class1.GetMessage("candleExpertQueue");
            Assert.AreNotEqual(1, result1);
            string result2 = Class1.GetMessage("candleExpertQueue");
            Assert.AreEqual(1, result2);
        }

        [TestMethod]
        public void TestMethod3()
        {
            //string result = Class1.Connect();
            string result2 = Class1.SendMessageToQueue("candleExpertQueue1", "test");
            //string result2 = Class1.GetMessage("candleExpertQueue1");
            Assert.AreEqual("test", result2);
        }

        [TestMethod]
        public void TestMethod4()
        {
            string result = Class1.Connect();
            Assert.AreEqual("test", result);
        }
    }
}
