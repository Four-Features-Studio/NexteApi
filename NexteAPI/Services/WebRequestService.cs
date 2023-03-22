using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using NexteAPI.Interfaces;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Asn1.Ocsp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace NexteAPI.Services
{
    public class WebRequestService : IWebRequestService
    {
        /// <summary>
        /// Make an async HTTP POST request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> POST(string url, object data, string contentType = "application/json", Dictionary<string, string> customHeaders = null)
        {
            HttpResponseMessage response;
            // POST or GET
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(3);
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                var model = JsonConvert.SerializeObject(data);

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = new StringContent(model, Encoding.UTF8);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                response = await client.SendAsync(request);
            }

            return response;
        }

        /// <summary>
        /// Make an async HTTP GET request
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GET(string url, Dictionary<string, string> query = default, Dictionary<string, string> customHeaders = null)
        {
            HttpResponseMessage response;
            // POST or GET
            using (var client = new HttpClient())
            {

                url = QueryHelpers.AddQueryString(url, query);

                client.Timeout = TimeSpan.FromSeconds(3);
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                
                response = await client.SendAsync(request);
            }

            return response;
        }
    }
}
