using Newtonsoft.Json;

namespace NexteAPI.DTO.AuthRequestes
{
    public class JoinRequest
    { 
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("selectedProfile")]
        public string SelectedProfile { get; set; }

        [JsonProperty("serverId")]
        public string ServerId { get; set; }
    }
}
