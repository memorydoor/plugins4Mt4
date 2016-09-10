using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using http4Mt4;

namespace http4Mt4UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string result = Class1.httpGetString("http://localhost:8081/mt4Rest/getFormingXabcds?symbol#");

            Assert.AreEqual(1, result);
        }
    }
}
