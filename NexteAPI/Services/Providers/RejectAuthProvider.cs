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
        public TypeAuthProvider Type { get; } = TypeAuthProvider.Reject;

        public async Task<AuthLoginResponse> LoginAsync(string username, string password)
        {
            return new AuthLoginResponse() { Successful = false, Message= "Auth Rejected" };
        }

        public async Task LogoutAsync(string accessToken)
        {
            return;
        }

        public async Task<HasJoinedResponse> HasJoinedAsync(string username, string serverId)
        {
            return null;
        }

        public async Task<bool> JoinAsync(string accessToken, string userUUID, string serverId)
        {
            return false;
        }

        public async Task<PrivilegesResponse> PrivilegesAsync(string userUUID)
        {
            return null;
        }

        public async Task<ProfileResponse> ProfileAsync(string userUUID)
        {
            return null;
        }

        public async Task<ProfilesResponse[]> ProfilesAsync(string[] users)
        {
            return null;
        }
    }
}
