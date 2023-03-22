using Newtonsoft.Json;

namespace NexteAPI.Entity
{
    public class UserProfile
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

    }
}
