using Newtonsoft.Json;
using NexteAPI.Entity;
using System.Text.Json.Serialization;

namespace NexteAPI.DTO
{
    public class MetadataResponse
    {
        [JsonProperty("meta")]
        public Metadata Meta { get; set; } = new Metadata();

        [JsonProperty("skinDomains")]
        public string[] SkinDomains { get; set; }

        [JsonProperty("signaturePublickey")]
        public string SignaturePublickey { get; set; }
    }
}
