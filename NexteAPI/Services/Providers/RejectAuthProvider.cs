using Microsoft.Extensions.Options;
using NexteAPI.Configs;
using NexteAPI.DTO.AuthResponses;
using NexteAPI.Entity;
using NexteAPI.Interfaces;
using System.Threading.Tasks;

namespace NexteAPI.Services.Providers
{
    public class RejectAuthProvider : IAuthProvider
    {
        IOptions<SystemConfig> options;
        public RejectAuthProvider(IOptions<SystemConfig> _options)
        {
            options = _options;
        }



        public async Task<UserProfile> LoginAsync(string username, string password)
        {
            throw new System.NotImplementedException();
        }
        public async Task LogoutAsync(string accessToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task<HasJoinedResponse> HasJoinedAsync(string username, string serverId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> JoinAsync(string accessToken, string userUUID, string serverId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<PrivilegesResponse> PrivilegesAsync(string userUUID)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ProfileResponse> ProfileAsync(string userUUID)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ProfilesResponse[]> ProfilesAsync(string[] users)
        {
            throw new System.NotImplementedException();
        }
    }
}
