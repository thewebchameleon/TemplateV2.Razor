using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TemplateV2.Infrastructure.HttpClients.Exceptions;
using TemplateV2.Infrastructure.HttpClients.Models;

namespace TemplateV2.Infrastructure.HttpClients
{
    public static class HttpHelper
    {
        public static async Task<HttpResponseMessage> Post(IHttpClientFactory _httpClientFactory, string serviceName, string method, string jsonData = null)
        {
            var httpClient = _httpClientFactory.CreateClient(serviceName);

            var baseUrl = httpClient.BaseAddress.ToString();
            var methodCallUrl = $"{baseUrl}{method}";

            var httpRequest = new HttpRequestMessage()
            {
                RequestUri = new Uri(methodCallUrl),
                Method = HttpMethod.Post,
            };

            if (!string.IsNullOrEmpty(jsonData))
            {
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                httpRequest.Content = content;
            }
            return await httpClient.SendAsync(httpRequest);
        }

        public static async Task<HttpResponseMessage> Post(IHttpClientFactory _httpClientFactory, string url, string jsonData = null)
        {
            var httpClient = _httpClientFactory.CreateClient(url);

            var httpRequest = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
            };

            if (!string.IsNullOrEmpty(jsonData))
            {
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                httpRequest.Content = content;
            }
            return await httpClient.SendAsync(httpRequest);
        }

        public static async Task<HttpResponseMessage> Get(IHttpClientFactory _httpClientFactory, string serviceName, string method)
        {
            var httpClient = _httpClientFactory.CreateClient(serviceName);

            var baseUrl = httpClient.BaseAddress.ToString();
            var methodCallUrl = $"{baseUrl}{method}";

            var httpRequest = new HttpRequestMessage()
            {
                RequestUri = new Uri(methodCallUrl),
                Method = HttpMethod.Get,
            };
            return await httpClient.SendAsync(httpRequest);
        }

        public static async Task<HttpResponseMessage> Get(IHttpClientFactory _httpClientFactory, string url)
        {
            var httpClient = _httpClientFactory.CreateClient(url);

            var httpRequest = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
            };
            return await httpClient.SendAsync(httpRequest);
        }

        public static T ReadMessage<T>(this HttpResponseMessage responseMessage)
        {
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;

            var httpCode = responseMessage.StatusCode;

            if (httpCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(responseData);
            }

            if (httpCode == HttpStatusCode.InternalServerError)
            {
                throw new InternalServiceErrorException(responseMessage.RequestMessage.RequestUri.ToString(), responseData);
            }
            throw new UnsupportedHttpCodeException(httpCode, responseMessage.ReasonPhrase);
        }
    }

}
