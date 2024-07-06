using HashGo.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Infrastructure.HttpHelper
{
    public class HttpHelper
    {
        private static HttpClient _httpClient = new HttpClient();
        private static HttpHelper _uniqueInstance = null;
        private static string? _token;
        private static readonly object locker = new object();

        private HttpHelper() { }

        public static HttpHelper  GetInstance(string url)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
            _httpClient.BaseAddress = new Uri(url);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            return _uniqueInstance;
        }

        public static HttpHelper GetInstance()
        {
            if (_uniqueInstance == null)
            {
                lock (locker)
                {
                    if (_uniqueInstance == null)
                    {
                        _httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
                        _httpClient.BaseAddress = new Uri(HashGoAppSettings.Url);
                        _httpClient.Timeout = TimeSpan.FromSeconds(30);
                        return _uniqueInstance = new HttpHelper();
                    }
                }
            }
            return _uniqueInstance;
        }

        public void SetToken(string token)
        {
            _token = token;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        public string Post(string requestBody, string url)
        {
            string result = string.Empty;
            try
            {
                StringContent? content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = _httpClient.PostAsync(url, content).Result)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                //NLogger.Error(ex);
            }
            return result;
        }
        public string Get(string url)
        {
            string result = string.Empty;
            try
            {
                _httpClient.Timeout = TimeSpan.FromSeconds(1);
                using (HttpResponseMessage response = _httpClient.GetAsync(url).Result)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                
            }
            return result;
        }

        public string PostAsyncWithoutAuth(string requestBody, string url)
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            var resultString = Post(requestBody, url);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            return resultString;
        }
    }
}
