using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NexteAPI.Interfaces
{
    public interface IWebRequestService
    {
        Task<HttpResponseMessage> POST(string url, object data, string contentType = "application/json", Dictionary<string, string> customHeaders = null);

        Task<HttpResponseMessage> GET(string url, Dictionary<string, string> query = default, Dictionary<string, string> customHeaders = null);
    }
}
