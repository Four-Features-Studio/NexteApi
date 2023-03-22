using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NexteAPI.Entity
{
    public class Server
    {
        [JsonProperty("ip")]
        public string Ip { get; set; } = "127.0.0.1";

        [JsonProperty("port")]
        public int Port { get; set; } = 25565;
    }
}
