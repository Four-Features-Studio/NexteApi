namespace NexteAPI.Configs
{
    public class SystemConfig
    {
        /// <summary>
        /// Домен сервера
        /// </summary>
        public string DomainSite { get; set; } = "http://localhost:5001/";
        public string ProjectName { get; set; } = "NexteLite Server";
        public string ProjectVersion { get; set; } = "0.0.1a";
        public string[] SkinDomains { get; set; } = { ".fourfeatures.ru", "fourfeatures.ru" };

        public AuthProviderOptions AuthProvider { get; set; }

        public TexturesOptions TexturesOptions { get; set; }

        public FileServiceOptions FileServiceOptions { get; set; }

        public KeysOptions KeysOptions { get; set; }

        
    }

}
