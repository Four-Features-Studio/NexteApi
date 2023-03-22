namespace NexteAPI.Configs
{
    public class AuthProviderOptions
    {
        public TypeAuthProvider Type { get; set; } = TypeAuthProvider.Json;
        public ConnectionOptions Connection { get; set; } = null!;

        public string JsonUrl { get; set; } = "https://example.com/";

        public TypePasswordHash PasswordHashMethod { get; set; } = TypePasswordHash.SHA1;
    }

    public enum TypePasswordHash
    {
        SHA1 = 0,
        BCrypt = 1,
        MD5 = 2,// эта поебень не рекомендованна к использованию
        DoubleMD5 = 3 // эта поебень не рекомендованна к использованию
    }

    public enum TypeAuthProvider
    {
        Accept = 0,
        Reject = 1,
        Json = 2,
        Database = 3 // эта поебень не рекомендованна к использованию
    }
}
