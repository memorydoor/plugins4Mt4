using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using json4Mt4;

namespace json4Mt4UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string json = @"{
                'count': 2,
                'xabcds': [
                {
                    'symbol': 'USDJPY',
                    'period': 240,
                    'description': 'test1'
                },
                {
                    'symbol': 'USDJPY',
                    'period': 240,
                    'description': 'test2'
                },
                {
                    'symbol': 'USDJPY',
                    'period': 60,
                    'description': 'test2'
                }
                ]
            }";

            String xpath = "$.xabcds[?(@.symbol == 'USDJPY' && @.period == 240)]";
            long result = Class1.jsonCount(json, xpath);
            Assert.AreEqual(2, result);

            string path = "$.xabcds[?(@.symbol == 'USDJPY' && @.period == 240)]";
            string result1 = Class1.getSimpleJson(json, path, 1);
            //Assert.AreEqual(2, result1);

            string json1 = @"               {
                    'symbol': 'USDJPY',
                    'period': 60,
                    'description': 'test2'
                }";

            path = "$.symbol";
            string result2 = Class1.getString(json1, path);
            Assert.AreEqual("USDJPY", result2);
            Assert.AreEqual(60, Class1.getInt(json1, "$.period"));
        }

        [TestMethod]
        public void testMethod2() {
            string jsonStr = "{\"count\":4,\"xabcds\":[{\"id\":-1909669210,\"symbol\":\"USDJPY\",\"period\":240,\"extremumType\":\"high\",\"x\":\"2016.07.27 00:00:00\",\"a\":\"2016.08.02 16:00:00\",\"b\":\"2016.08.08 12:00:00\",\"c\":\"2016.08.16 12:00:00\",\"perfectD\":104.168,\"d\":\"NA\"},{\"id\":476840296,\"symbol\":\"USDJPY\",\"period\":240,\"extremumType\":\"high\",\"x\":\"2016.07.25 00:00:00\",\"a\":\"2016.08.02 16:00:00\",\"b\":\"2016.08.08 12:00:00\",\"c\":\"2016.08.16 12:00:00\",\"perfectD\":104.168,\"d\":\"NA\"},{\"id\":954892012,\"symbol\":\"USDJPY\",\"period\":240,\"extremumType\":\"high\",\"x\":\"2016.07.21 00:00:00\",\"a\":\"2016.08.02 16:00:00\",\"b\":\"2016.08.08 12:00:00\",\"c\":\"2016.08.16 12:00:00\",\"perfectD\":104.297,\"d\":\"NA\"},{\"id\":788195411,\"symbol\":\"USDJPY\",\"period\":1440,\"extremumType\":\"high\",\"x\":\"2016.01.29 00:00:00\",\"a\":\"2016.05.03 00:00:00\",\"b\":\"2016.05.30 00:00:00\",\"c\":\"2016.06.24 00:00:00\",\"perfectD\":116.715,\"d\":\"NA\"}]}";
            String xpath = "$.xabcds[?(@.symbol == 'USDJPY' && @.period == 240)]";
            int result = Class1.jsonCount(jsonStr, xpath);
            Assert.AreEqual(3, result);
        }
    }
}