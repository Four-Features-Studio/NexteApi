using CommandLine;

namespace NexteAPI.CommandHandlers
{
    [Verb("createprofile", HelpText = "Создание профиля")]
    public class CreateProfileOptions
    {
        [Option('n', "name", Required = true, HelpText = "Название профиля")]
        public string nameclient { get; set; }

        [Option('v', "version", Required = true, HelpText = "Версия клиента для профиля")]
        public string versionclient { get; set; }
    }

    //[Verb("downloadclient", HelpText = "Скачивание заготовки клиента игры указанной версии")]
    //public class DownloadClientOptions
    //{
    //    [Option('m', "mojang", Required = false, HelpText = "Использовать зеркала Mojang")]
    //    public bool mojang { get; set; }

    //    [Option('v', "version", Required = true, HelpText = "Версия клиента")]
    //    public string versionclient { get; set; }

    //    [Option('n', "name", Required = true, HelpText = "Название клиента")]
    //    public string nameclient { get; set; }
    //}

    //[Verb("downloadassets", HelpText = "Скачивание ассетов клиента игры указанной версии")]
    //public class DownloadAssetsOptions
    //{
    //    [Option('m', "mojang", Required = false, HelpText = "Использовать зеркала Mojang")]
    //    public bool mojang { get; set; }

    //    [Option('v', "version", Required = true, HelpText = "Версия ассетов")]
    //    public string versionclient { get; set; }
    //}

    [Verb("syncall", HelpText = "Синхронизация профилоей и папки с файлами клиентов")]
    public class SyncAllOptions
    {
    }

    [Verb("syncprofiles", HelpText = "Синхронизация папки профилей")]
    public class SyncProfilesOptions
    {
    }

    [Verb("syncupdates", HelpText = "Синхронизация папки обновлений")]
    public class SyncUpdatesOptions
    {
    }

    [Verb("gravit", HelpText = "Парень, ты уверен что хочешь это использовать?")]
    public class GravitOptions
    {
    }
}
