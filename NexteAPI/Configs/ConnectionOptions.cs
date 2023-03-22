namespace NexteAPI.Configs
{
    public class ConnectionOptions
    {
        public TypeDbProvider Type { get; set; } = TypeDbProvider.MySQL;

        public string ConnectionString { get; set; }
    }
    public enum TypeDbProvider
    {
        Sqlite = 0,
        MySQL = 1,
        PostgreSQL = 2
    }
}
