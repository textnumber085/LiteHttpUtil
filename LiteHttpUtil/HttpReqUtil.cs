using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LiteHttpUtil
{
    public class HttpReqUtil
    {
        /// <summary>
        /// 发起的HTTP请求的默认超时时间
        /// </summary>
        public static TimeSpan defaultTimeOut = TimeSpan.FromSeconds(60);

        #region Get

        /// <summary>
        /// 发起一个HTTP GET请求
        /// </summary>
        /// <param name="url">目标url</param>
        /// <returns></returns>
        public static string Get(string url, TimeSpan? timeout = null)
        {
            try
            {
                return HttpRequest(HttpMethodEnum.Get, url, null, timeout).Content.ReadAsStringAsync().Result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 发起一个HTTP GET请求
        /// </summary>
        /// <param name="url">目标url</param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> GetAsync(HttpClient client, string url, TimeSpan? timeout = null)
        {
            try
            {
                return HttpRequestAsync(client, HttpMethodEnum.Get, url, null, timeout);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Post

        /// <summary>
        /// 发起一个HTTP Post请求
        /// </summary>
        /// <param name="url">目标url</param>
        /// <returns></returns>
        public static string Post(string url, List<KeyValuePair<string, string>> postDataPair, TimeSpan? timeout = null)
        {
            try
            {
                return HttpRequest(HttpMethodEnum.Post, url, postDataPair, timeout).Content.ReadAsStringAsync().Result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 发起一个HTTP Post请求
        /// </summary>
        /// <param name="url">目标url</param>
        /// <returns></returns>
        public static string Post(string url, string postData, TimeSpan? timeout = null)
        {
            try
            {
                return HttpRequest(url, HttpMethodEnum.Post, postData, timeout).Content.ReadAsStringAsync().Result;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 发起一个HTTP Post请求
        /// </summary>
        /// <param name="url">目标url</param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> PostAsync(HttpClient client, string url, List<KeyValuePair<string, string>> postDataPair, TimeSpan? timeout = null)
        {
            try
            {
                return HttpRequestAsync(client, HttpMethodEnum.Post, url, postDataPair, timeout);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 发起一个HTTP Post请求
        /// </summary>
        /// <param name="url">目标url</param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> PostAsync(HttpClient client, string url, string postData, TimeSpan? timeout = null)
        {
            try
            {
                return HttpRequestAsync(client, url, HttpMethodEnum.Post, postData, timeout);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Put

        /// <summary>
        /// 发起一个HTTP Put请求
        /// </summary>
        /// <param name="url">目标url</param>
        /// <returns></returns>
        public static string Put(string url, string postData, TimeSpan? timeout = null)
        {
            try
            {
                return HttpRequest(url, HttpMethodEnum.Put, postData, timeout).Content.ReadAsStringAsync().Result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 发起一个HTTP Put请求
        /// </summary>
        /// <param name="url">目标url</param>
        /// <returns></returns>
        public static string Put(string url, List<KeyValuePair<string, string>> postDataPair, TimeSpan? timeout = null)
        {
            try
            {
                return HttpRequest(HttpMethodEnum.Put, url, postDataPair, timeout).Content.ReadAsStringAsync().Result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 发起一个HTTP Put请求
        /// </summary>
        /// <param name="url">目标url</param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> PutAsync(HttpClient client, string url, List<KeyValuePair<string, string>> postDataPair, TimeSpan? timeout = null)
        {
            try
            {
                return HttpRequestAsync(client, HttpMethodEnum.Put, url, postDataPair, timeout);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 发起一个HTTP Put请求
        /// </summary>
        /// <param name="url">目标url</param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> PutAsync(HttpClient client, string url, string postData, TimeSpan? timeout = null)
        {
            try
            {
                return HttpRequestAsync(client, url, HttpMethodEnum.Put, postData, timeout);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// 发起一个HTTP DELETE请求
        /// </summary>
        /// <param name="url">目标url</param>
        /// <returns></returns>
        public static string Delete(string url, TimeSpan? timeout = null)
        {
            try
            {
                return HttpRequest(HttpMethodEnum.Delete, url, null, timeout).Content.ReadAsStringAsync().Result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 发起一个HTTP DELETE请求
        /// </summary>
        /// <param name="url">目标url</param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> DeleteAsync(HttpClient client, string url, TimeSpan? timeout = null)
        {
            try
            {
                return HttpRequestAsync(client, HttpMethodEnum.Delete, url, null, timeout);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Base

        /// <summary>
        /// 发起一个HTTP请求
        /// </summary>
        /// <param name="verb">HTTP方法</param>
        /// <param name="url">目标url</param>
        /// <param name="postDataPair">postData</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> HttpRequestAsync(HttpClient client, HttpMethodEnum verb, string url, List<KeyValuePair<string, string>> postDataPair, TimeSpan? timeout = null)
        {
            client.Timeout = timeout ?? defaultTimeOut;
            if (url.StartsWith("https"))
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true; //Prevent error: Could not establish trust relationship for the SSL/TLS secure channel
            }
            postDataPair = postDataPair ?? new List<KeyValuePair<string, string>>();
            FormUrlEncodedContent pairContent = new FormUrlEncodedContent(postDataPair);
            switch (verb)
            {
                case HttpMethodEnum.Get: return client.GetAsync(url);
                case HttpMethodEnum.Post: return client.PostAsync(url, pairContent);
                case HttpMethodEnum.Put: return client.PutAsync(url, pairContent);
                case HttpMethodEnum.Delete: return client.DeleteAsync(url);
                default: return client.GetAsync(url);
            }
        }

        /// <summary>
        /// 发起一个HTTP请求
        /// </summary>
        /// <param name="verb">HTTP方法</param>
        /// <param name="url">目标url</param>
        /// <param name="postDataPair">postData</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        public static HttpResponseMessage HttpRequest(HttpMethodEnum verb, string url, List<KeyValuePair<string, string>> postDataPair, TimeSpan? timeout = null)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = timeout ?? defaultTimeOut;
                if (url.StartsWith("https"))
                {
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true; //Prevent error: Could not establish trust relationship for the SSL/TLS secure channel
                }
                postDataPair = postDataPair ?? new List<KeyValuePair<string, string>>();
                FormUrlEncodedContent pairContent = new FormUrlEncodedContent(postDataPair);
                switch (verb)
                {
                    case HttpMethodEnum.Get: return client.GetAsync(url).Result;
                    case HttpMethodEnum.Post: return client.PostAsync(url, pairContent).Result;
                    case HttpMethodEnum.Put: return client.PutAsync(url, pairContent).Result;
                    case HttpMethodEnum.Delete: return client.DeleteAsync(url).Result;
                    default: return client.GetAsync(url).Result;
                }
            }
            
        }

        /// <summary>
        /// 发起一个HTTP请求
        /// </summary>
        /// <param name="verb">HTTP方法</param>
        /// <param name="url">目标url</param>
        /// <param name="postDataJson">json格式的postData字符串</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> HttpRequestAsync(HttpClient client, string url, HttpMethodEnum verb, string postDataJson, TimeSpan? timeout = null)
        {
            client.Timeout = timeout ?? defaultTimeOut;
            if (url.StartsWith("https"))
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true; //Prevent error: Could not establish trust relationship for the SSL/TLS secure channel
            }
            HttpContent jsonContent = new StringContent(postDataJson ?? "", Encoding.UTF8);
            jsonContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            switch (verb)
            {
                case HttpMethodEnum.Get: return client.GetAsync(url);
                case HttpMethodEnum.Post: return client.PostAsync(url, jsonContent);
                case HttpMethodEnum.Put: return client.PutAsync(url, jsonContent);
                case HttpMethodEnum.Delete: return client.DeleteAsync(url);
                default: return client.GetAsync(url);
            }
        }

        /// <summary>
        /// 发起一个HTTP请求
        /// </summary>
        /// <param name="verb">HTTP方法</param>
        /// <param name="url">目标url</param>
        /// <param name="postDataJson">json格式的postData字符串</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        public static HttpResponseMessage HttpRequest(string url, HttpMethodEnum verb, string postDataJson, TimeSpan? timeout = null)
        {
            using(var client = new HttpClient())
            {
                client.Timeout = timeout ?? defaultTimeOut;
                if(url.StartsWith("https"))
                {
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true; //Prevent error: Could not establish trust relationship for the SSL/TLS secure channel
                }
                HttpContent jsonContent = new StringContent(postDataJson ?? "", Encoding.UTF8);
                jsonContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                switch (verb)
                {
                    case HttpMethodEnum.Get: return client.GetAsync(url).Result;
                    case HttpMethodEnum.Post: return client.PostAsync(url, jsonContent).Result;
                    case HttpMethodEnum.Put: return client.PutAsync(url, jsonContent).Result;
                    case HttpMethodEnum.Delete: return client.DeleteAsync(url).Result;
                    default: return client.GetAsync(url).Result;
                }
            }            
        }

        #endregion
    }
}
