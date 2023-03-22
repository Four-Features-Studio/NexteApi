using Newtonsoft.Json;

namespace NexteAPI.Entity
{
    public class FileEntity
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("size")]
        public double Size { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
