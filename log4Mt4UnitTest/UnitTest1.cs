using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace log4Mt4UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            log4Mt4.Class1.initLog4Mt4("appName", "TRUE");

            log4Mt4.Class1.info("TRUE");
            log4Mt4.Class1.info("TRU1");
            log4Mt4.Class1.info("TRU3");
        }
    }
}
