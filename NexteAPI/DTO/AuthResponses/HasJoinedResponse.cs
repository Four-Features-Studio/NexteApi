using NexteAPI.Entity;
using System.Net;

namespace NexteAPI.DTO.AuthResponses
{
    public class HasJoinedResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public Profile Data { get; set; }

    }
}
