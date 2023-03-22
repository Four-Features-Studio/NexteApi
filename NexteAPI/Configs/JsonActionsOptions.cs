namespace NexteAPI.Configs
{
    public class JsonActionsOptions
    {
        public string Login { get; set; } = "api/account/login";
        public string Logout { get; set; } = "api/account/logout";
        public string HasJoined { get; set; } = "session/minecraft/hasjoined";
        public string Join { get; set; } = "session/minecraft/join";
        public string Privileges { get; set; } = "player/attributes";
        public string Profile { get; set; } = "session/minecraft/profile";
        public string Profiles { get; set; } = "profiles/minecraft";
    }

}
