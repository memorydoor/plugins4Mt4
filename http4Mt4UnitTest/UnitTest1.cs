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

        [TestMethod]
        public void test_httpPostJsonString()
        {
            string jsonContentString = @"
{
	""alertTimes1"" : 0,

    ""latestAlertDate"" : null,
	""firstAlertDate"" : null,
	""isPatternInvalid"" : false,
	""traded"" : false,
	""monitoring"" : false,
	""monitoringUrl"" : null
}
";

            string result = Class1.httpPostJsonString("http://localhost:8081/xabcdPatternEditDistanceComments", 
                jsonContentString);

            
            Assert.AreEqual("201", result);
        }

        

        [TestMethod]
        public void test_httpPutString()
        {
            string result = Class1.httpPutString(
                "http://localhost:8081/xabcdPatternEditDistances/52942/comment", 
                "http://localhost:8081/xabcdPatternEditDistanceComments/3");

            Assert.AreEqual("204", result);
        }

        [TestMethod]
        public void test_httpPatchJsonString()
        {
            string result = Class1.httpPatchJsonString(
                "http://candle:8080/candleSyncs/272",
                "{\"isSyncing\":true}");

            Assert.AreEqual("204", result);
        }
    }
}
