using Microsoft.AspNetCore.Mvc;
using NexteAPI.DTO.AuthRequestes;
using NexteAPI.DTO.AuthResponses;
using NexteAPI.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace NexteAPI.Interfaces
{
    public interface IAuthProvider
    {
        Task<UserProfile> LoginAsync(string username, string password);
        Task LogoutAsync(string accessToken);
        Task<bool> JoinAsync(string accessToken, string userUUID, string serverId);
        Task<HasJoinedResponse> HasJoinedAsync(string username, string serverId);
        Task<PrivilegesResponse> PrivilegesAsync(string accessToken);
        Task<ProfileResponse> ProfileAsync(string uuid);
        Task<ProfilesResponse[]> ProfilesAsync(string[] users);

    }
}
