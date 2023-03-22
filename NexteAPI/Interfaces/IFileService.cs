using NexteAPI.Commands;
using NexteAPI.Entity;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace NexteAPI.Interfaces
{
    public interface IFileService
    {
        Dictionary<string, ClientProfile> Profiles { get; set; }
        Dictionary<string, List<FileEntity>> Clients { get; set; }
        Dictionary<string, AssetsIndex> AssetsIndexes { get; set; }

        Task SyncProfiles();
        Task SyncUpdates();
        Task SyncAll();
        Task CreateProfile(CreateProfileCommand args);


        Task<string> LoadPrivateKey();
        Task<string> LoadPublicKey();
    }
}
