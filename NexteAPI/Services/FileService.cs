using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;
using log4net;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlX.XDevAPI;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using NexteAPI.Commands;
using NexteAPI.Configs;
using NexteAPI.Entity;
using NexteAPI.Interfaces;
using Org.BouncyCastle.Crypto.Modes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace NexteAPI.Services
{
    public class FileService : IFileService
    {
        SystemConfig options;
        ILogger<FileService> logger;

        public FileService(IOptions<SystemConfig> _options, ILogger<FileService> _logger)
        {
            options = _options.Value;
            logger = _logger;
        }

        public Dictionary<string, ClientProfile> Profiles { get; set; } = new Dictionary<string, ClientProfile>();
        public Dictionary<string, List<FileEntity>> Clients { get; set; } = new Dictionary<string, List<FileEntity>>();
        public Dictionary<string, AssetsIndex> AssetsIndexes { get; set; } = new Dictionary<string, AssetsIndex>();

        public const string AssetsIndexesFolder = "indexes";


        private string RootPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, options.FileServiceOptions.RootPath);

        private string ProfilesPath => Path.Combine(RootPath, options.FileServiceOptions.FolderNameProfiles);
        private string UpdatesPath => Path.Combine(RootPath, options.FileServiceOptions.FolderNameUpdates);
        private string ClientsPath => Path.Combine(UpdatesPath, options.FileServiceOptions.FolderNameClientsUpdates);
        private string AssetsPath => Path.Combine(UpdatesPath, options.FileServiceOptions.FolderNameAssetsUpdates);

        private string AssetsIndexesPath => Path.Combine(AssetsPath, AssetsIndexesFolder);

        private string PublicKeyPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, options.KeysOptions.KeysFolder, options.KeysOptions.PublicKeyName);
        private string PrivateKeyPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, options.KeysOptions.KeysFolder, options.KeysOptions.PrivateKeyName);


        #region [Create Profile]
        public async Task CreateProfile(CreateProfileCommand args)
        {
            logger.LogInformation($"Создание профиля {args.Name} {args.Version}v");

            var profile = new ClientProfile()
            {
                ProfileId = Guid.NewGuid().ToString(),
                Title = args.Name,
                Version = args.Version,
                SortIndex = 0,
                Dir = args.Name,
                Server = new Server(),
                AssetIndex = args.Version,
                UpdatesList = new List<string>()
                {
                    "libraries",
                    "natives",
                    "mods",
                    "configs",
                    "resourcespacks",
                    "minecraft.jar",
                    "forge.jar",
                    "liteloader.jar"
                },
                IgnoreList = new List<string>(),
                JvmArgs = new List<string>(),
                MainClass = "net.minecraft.client.main.Main",
                HideProfile = false,
                Presets = new List<ServerPreset>()
            };

            new FileInfo(ProfilesPath).Directory?.Create();

            var path = Path.Combine(ProfilesPath, $"{args.Name}.json");

            if (File.Exists(path))
            {
                logger.LogError("Указанное название профиля занято, пожалуйста укажите другое.");
                return;
            }

            using (var fs = new FileStream(path, FileMode.Create))
            {
                var json_profile = JsonConvert.SerializeObject(profile, Formatting.Indented);
                var data = Encoding.UTF8.GetBytes(json_profile);
                await fs.WriteAsync(data, 0, data.Length);
            }

            logger.LogInformation($"Профиль {args.Name} успешно создан");

            await SyncProfiles();
        }
        #endregion

        public async Task SyncAll()
        {
            var sync_profiles = SyncProfiles();
            var sync_clients = SyncUpdates();

            await Task.WhenAll(sync_clients, sync_profiles);
        }

        public async Task SyncProfiles()
        {
            logger.LogInformation("Синхронизация профилей");

            Profiles = new Dictionary<string, ClientProfile>();

            new FileInfo(ProfilesPath).Directory?.Create();

            var profiles_info = new DirectoryInfo(ProfilesPath);

            var profiles = profiles_info.GetFiles("*.json");

            if (profiles.Length == 0)
            {
                logger.LogError("При синхронизации не было найденно ни одно профиля. Создайте их или проверти настройки путей");
                return;
            }

            foreach (var profile in profiles)
            {
                using (StreamReader file = new StreamReader(profile.Open(FileMode.Open)))
                {
                    string json_profile = file.ReadToEnd();
                    ClientProfile profile_entity = JsonConvert.DeserializeObject<ClientProfile>(json_profile);
                    Profiles.Add(profile_entity.ProfileId, profile_entity);

                    logger.LogInformation($"Профиль {profile_entity.Title} синхронизирован");

                }
            }

            logger.LogInformation("Синхронизация профилей завершена");
        }

        public async Task SyncUpdates()
        {
            logger.LogInformation("Синхронизация ассетов");

            Clients = new Dictionary<string, List<FileEntity>>();
            AssetsIndexes = new Dictionary<string, AssetsIndex>();

            var stopwatcher = new Stopwatch();
            stopwatcher.Start();
            //assets
            if (!Directory.Exists(AssetsPath))
            {
                logger.LogError("Ассетов не существует, добавте их или проверти настройки путей");
                return;
            }

            if (!Directory.Exists(AssetsIndexesPath))
            {
                logger.LogError("В папке с ассетами не существует ассетов, пожалуйста восстановите ассеты клиентов");
                return;
            }

            var indexes_info = new DirectoryInfo(AssetsIndexesPath);
            var indexes = indexes_info.GetFiles("*.json");

            if(indexes.Length == 0)
            {
                logger.LogError("Assets indexes не найдены");
                return;
            }

            foreach(var index in indexes)
            {

                using (var fs = new FileStream(index.FullName, FileMode.OpenOrCreate))
                {
                    using (var sr = new StreamReader(fs, Encoding.UTF8))
                    {
                        var dataRaw = sr.ReadToEnd();
                        var data = JsonConvert.DeserializeObject<AssetsIndex>(dataRaw, new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace });

                        if (data != null)
                        {
                            var key = Path.GetFileNameWithoutExtension(index.Name);
                            AssetsIndexes.Add(key, data);
                        }
                    }
                }
            }
            logger.LogInformation("Индексы ассетов загружены");

            var clients_info = new DirectoryInfo(ClientsPath);
            var clients = clients_info.GetDirectories();

            if (clients.Length == 0)
            {
                logger.LogError("Игровые клиенты не найдены");
                return;
            }

            foreach(var client in clients)
            {
                var dirname = client.Name;
                var profileId = Profiles.FirstOrDefault(x => x.Value.Dir == dirname).Key;

                var files_entities = new List<FileEntity>();

                if (!string.IsNullOrEmpty(profileId))
                {
                    var client_files = client.GetFiles("*", SearchOption.AllDirectories);

                    for(int i =0; i < client_files.Length; i++ )
                    {
                        var file = client_files[i];
                        var file_name = file.Name;
                        var file_path = file.FullName;

                        FileEntity entity = new FileEntity();

                        var replace_path = Path.Combine(ClientsPath, dirname);

                        var clear_path = file_path.Replace(replace_path, "");

                        if (clear_path.StartsWith("\\") || clear_path.StartsWith("/"))
                            clear_path = clear_path.Substring(1);

                        var file_data = await File.ReadAllBytesAsync(file_path);

                        entity.Name = file_name;
                        entity.Path = clear_path;
                        entity.Hash = Hasher.ComputeSHA1(file_data);
                        entity.Size = (double)file.Length;
                        entity.Url = UrlCombiner.Combine(options.FileServiceOptions.RouteToUpdates, options.FileServiceOptions.FolderNameClientsUpdates, dirname, clear_path);

                        files_entities.Add(entity);
                    }

                    Clients.Add(profileId, files_entities);
                }
                else
                {
                    continue;
                }

            }

            stopwatcher.Stop();

            logger.LogInformation($"Синхронизация ассетов и клиентских файлов завершенна за {stopwatcher.ElapsedMilliseconds.ToString()}ms");

        }

        public async Task<string> LoadPrivateKey()
        {
            logger.LogInformation($"Загружаю приватный ключ");

            new FileInfo(PrivateKeyPath).Directory?.Create();

            using (var fs = new FileStream(PrivateKeyPath, FileMode.OpenOrCreate))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    var data = await sr.ReadToEndAsync();
                 
                    if (!string.IsNullOrEmpty(data))
                    {
                        logger.LogInformation($"Ключ загружен");
                        return data;
                    }

                    throw new KeyNotFoundException("Внимания, приватный ключ не найден, дальнейшая работа не возможна");
                }
            }
        }
        public async Task<string> LoadPublicKey()
        {
            logger.LogInformation($"Загружаю публичный ключ");

            new FileInfo(PublicKeyPath).Directory?.Create();

            using (var fs = new FileStream(PublicKeyPath, FileMode.OpenOrCreate))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    var data = await sr.ReadToEndAsync();

                    if (!string.IsNullOrEmpty(data))
                    {
                        logger.LogInformation($"Ключ загружен");
                        return data;
                    }

                    throw new KeyNotFoundException("Внимания, публичный ключ не найден, дальнейшая работа не возможна");
                }
            }
        }
    }
}
