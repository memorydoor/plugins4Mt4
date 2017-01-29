using RGiesecke.DllExport;
using System;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;

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

        [DllExport("httpPostJsonString", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string httpPostJsonString(
            [MarshalAs(UnmanagedType.LPWStr)]String url,
            [MarshalAs(UnmanagedType.LPWStr)]String jsonContentString)
        {
            HttpClient httpClient = new HttpClient();

            try
            {
                var content = new StringContent(jsonContentString, Encoding.UTF8, "application/json");
                var response = httpClient.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    // by calling .Result you are performing a synchronous call
                    var responseContent = response.Content;

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    

                    return response.Headers.Location.ToString();
                    //Console.WriteLine(responseString);
                }
                else
                {
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

        [DllExport("httpPutString", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string httpPutString(
            [MarshalAs(UnmanagedType.LPWStr)]String url,
            [MarshalAs(UnmanagedType.LPWStr)]String contentString)
        {
            HttpClient httpClient = new HttpClient();

            try
            {
                var content = new StringContent(contentString, Encoding.UTF8, "text/uri-list");
                var response = httpClient.PutAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    // by calling .Result you are performing a synchronous call
                    var responseContent = response.Content;

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;


                    return (int)response.StatusCode + "";
                    //Console.WriteLine(responseString);
                }
                else
                {
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


        [DllExport("httpPutJsonString", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string httpPutJsonString(
            [MarshalAs(UnmanagedType.LPWStr)]String url,
            [MarshalAs(UnmanagedType.LPWStr)]String contentString)
        {
            HttpClient httpClient = new HttpClient();

            try
            {
                var content = new StringContent(contentString, Encoding.UTF8, "application/json");
                var response = httpClient.PutAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    // by calling .Result you are performing a synchronous call
                    var responseContent = response.Content;

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;


                    return (int)response.StatusCode + "";
                    //Console.WriteLine(responseString);
                }
                else
                {
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

        [DllExport("httpPatchJsonString", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string httpPatchJsonString(
            [MarshalAs(UnmanagedType.LPWStr)]String url,
            [MarshalAs(UnmanagedType.LPWStr)]String contentString)
        {
            HttpClient httpClient = new HttpClient();

            try
            {

                var content = new StringContent(contentString, Encoding.UTF8, "application/json");

                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = new HttpMethod("PATCH"),
                    RequestUri = new Uri(url),
                    Content = content
                };

               
                var response = httpClient.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    // by calling .Result you are performing a synchronous call
                    var responseContent = response.Content;

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;


                    return (int)response.StatusCode + "";
                    //Console.WriteLine(responseString);
                }
                else
                {
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
