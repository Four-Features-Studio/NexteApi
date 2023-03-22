using Newtonsoft.Json;

namespace NexteAPI.DTO.AuthResponses
{
    public class ProfilesResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
