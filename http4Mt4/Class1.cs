using RGiesecke.DllExport;
using System;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace http4Mt4
{
    public class Class1
    {

        //[DllExport("GetMessageFromQueue", CallingConvention = CallingConvention.StdCall)]
        //[return: MarshalAs(UnmanagedType.LPWStr)]
        //public static string GetMessageFromQueue([MarshalAs(UnmanagedType.LPWStr)] string queueName)

        [DllExport("httpGetString", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string httpGetString([MarshalAs(UnmanagedType.LPWStr)]String url)
        {
            HttpClient httpClient = new HttpClient();

            try
            {
                var response = httpClient.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    // by calling .Result you are performing a synchronous call
                    var responseContent = response.Content;

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    return responseString;
                    //Console.WriteLine(responseString);
                }
                else {
                    return "error";
                }
                
            }
            catch (Exception ex)
            {
                string checkResult = "Error " + ex.ToString();
                httpClient.Dispose();
                return checkResult;
            }
        }
    }
}
