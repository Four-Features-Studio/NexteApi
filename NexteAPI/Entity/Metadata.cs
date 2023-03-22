using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg.Sig;
using System;

namespace NexteAPI.Entity
{
    public class Metadata
    {
        [JsonProperty("id")]
        public string ServerName { get; set; } = "Nexte Lite Launcher";

        [JsonProperty("implementationName")]
        public string ImplementationName { get; set; } = "nexte-lite-launcher";

        [JsonProperty("implementationVersion")]
        public string ImplementationVersion { get; set; } = "0.0.1";

        [JsonProperty("feature.no_mojang_namespace")]
        public bool NoMojang { get; set; } = true;

        [JsonProperty("feature.privileges_api")]
        public bool PrivilegesApi { get; set; } = true;
    }
}
