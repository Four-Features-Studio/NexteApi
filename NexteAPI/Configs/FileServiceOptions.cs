namespace NexteAPI.Configs
{
    public class FileServiceOptions
    {
        public string RootPath { get; set; } = "wwwroot";
        public string FolderNameUpdates { get; set; } = "updates";
        public string FolderNameClientsUpdates { get; set; } = "Clients";
        public string FolderNameAssetsUpdates { get; set; } = "Assets";
        public string FolderNameProfiles { get; set; } = "profiles";
        public string RouteToUpdates { get; set; } = "/files";
        public bool EnableDirectoryBrowsing { get; set; } = true;
        public string[] Mirrors { get; set; }
    }
}
