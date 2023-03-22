using NexteAPI.Entity;
using System.Threading.Tasks;

namespace NexteAPI.Interfaces
{
    public interface ITexturesService
    {
        Task<Profile> GetTextures(string uuidProfile, string username, string skinUrl, string cloakUrl);
    }
}
