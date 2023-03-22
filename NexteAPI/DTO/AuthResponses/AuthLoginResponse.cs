using NexteAPI.Entity;

namespace NexteAPI.DTO.AuthResponses
{
    public class AuthLoginResponse
    {
        public bool Successful { get; set; }
        public string Message { get; set; }
        public UserProfile Profile { get; set; }
    }
}
