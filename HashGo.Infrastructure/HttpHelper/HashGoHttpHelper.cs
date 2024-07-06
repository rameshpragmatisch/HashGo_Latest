using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Infrastructure.HttpHelper
{
    public class DineGoHttpHelper
    {
        private static readonly object locker = new object();
        private static readonly HttpClient httpClient = new HttpClient();
        private static DineGoHttpHelper uniqueInstance;

        private DineGoHttpHelper() { }

        public static DineGoHttpHelper GetInstance(string url, string deviceId, string userId = "1")
        {
            if (uniqueInstance == null)
            {
                lock (locker)
                {
                    if (uniqueInstance == null)
                    {
                        httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
                        httpClient.DefaultRequestHeaders.Add("DeviceId", deviceId);
                        httpClient.DefaultRequestHeaders.Add("UserId", userId);
                        httpClient.BaseAddress = new Uri(url);
                        httpClient.Timeout = TimeSpan.FromSeconds(30);
                        return uniqueInstance = new DineGoHttpHelper();
                    }
                }
            }
            return uniqueInstance;
        }

        public string PostAsync(string requestBody, string url)
        {
            string result = string.Empty;
            try
            {
                StringContent? content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = httpClient.PostAsync(url, content).Result)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                    System.Net.HttpStatusCode st = response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                //NLogger.Error(ex);
            }
            return result;
        }

        public string GetAsync(string url)
        {
            string result = string.Empty;
            try
            {
                using (HttpResponseMessage response = httpClient.GetAsync(url).Result)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                    System.Net.HttpStatusCode st = response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                //NLogger.Error(ex);
            }
            return result;
        }
    }
}
