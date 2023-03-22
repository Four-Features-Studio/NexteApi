using Newtonsoft.Json;
using System.Collections.Generic;

namespace NexteAPI.Entity
{
    public partial class AssetsIndex
    {
        [JsonProperty("objects")]
        public Dictionary<string, Asset> Objects { get; set; }
    }

    public partial class Asset
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }
    }

    public partial class AssetsIndex
    {
        public static AssetsIndex FromJson(string json) => JsonConvert.DeserializeObject<AssetsIndex>(json);
    }
}
