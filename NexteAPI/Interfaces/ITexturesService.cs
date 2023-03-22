using NexteAPI.Entity;
using System.Threading.Tasks;

namespace NexteAPI.Interfaces
{
    public interface ITexturesService
    {
        string GetSkin(string username);
        string GetCloak(string username);

        Task<Profile> GetTextures(string uuidProfile, string username, string skinUrl, string cloakUrl);
    }
}
