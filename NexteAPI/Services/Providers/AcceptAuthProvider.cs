using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NexteAPI.Configs;
using NexteAPI.DTO.AuthResponses;
using NexteAPI.Entity;
using NexteAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexteAPI.Services.Providers
{
    public class AcceptAuthProvider : IAuthProvider
    {
        IOptions<SystemConfig> options;
        ITexturesService textures;
        public AcceptAuthProvider(IOptions<SystemConfig> _options, ITexturesService _textures)
        {
            options = _options;
            textures = _textures;
        }

        public TypeAuthProvider Type { get; } = TypeAuthProvider.Accept;

        public List<UserData> Users = new List<UserData>();


        public async Task<AuthLoginResponse> LoginAsync(string username, string password)
        {
            var data = new UserData()
            {
                Username = username,
                Uuid = Guid.NewGuid().ToString(),
                AccessToken = Guid.NewGuid().ToString()
            };

            if(Users.Any(x => x.Uuid == data.Uuid))
            {
                Users.RemoveAll(x => x.Uuid == data.Uuid);
            }

            Users.Add(data);

            var result = new UserProfile()
            {
                Username = data.Username,
                Uuid = data.Uuid,
                AccessToken = data.AccessToken
            };

            return new AuthLoginResponse() { Successful = true, Message = string.Empty, Profile = result };
        }

        public async Task LogoutAsync(string accessToken)
        {
            if (Users.Any(x => x.AccessToken == accessToken))
            {
                Users.RemoveAll(x => x.AccessToken == accessToken);
            }
        }

        public async Task<HasJoinedResponse> HasJoinedAsync(string username, string serverId)
        {
            var user = Users.FirstOrDefault(x => x.Username == username && x.ServerId == serverId);

            if (user is null)
                return new HasJoinedResponse()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                };

            var profile = await textures.GetTextures(user.Uuid.ToString(), user.Username, textures.GetSkin(username), textures.GetCloak(username));

            var result = new HasJoinedResponse()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = profile
            };

            return result;

        }

        public async Task<bool> JoinAsync(string accessToken, string userUUID, string serverId)
        {
            var user = Users.FirstOrDefault(x => x.Uuid.ToString() == userUUID && x.AccessToken == accessToken);

            if (user is null)
                return false;

            user.ServerId = serverId;

            return true;
        }

        public async Task<PrivilegesResponse> PrivilegesAsync(string userUUID)
        {
            if (!Users.Any(x => x.Uuid.ToString() == userUUID))
                return null;

            var result = new PrivilegesResponse()
            {
                OnlineChat = true,
                MultiplayerServer = true,
                MultiplayerRealms = true,
                Telemetry = true
            };

            return result;
        }

        public async Task<ProfileResponse> ProfileAsync(string userUUID)
        {
            var user = Users.FirstOrDefault(x => x.Uuid.ToString() == userUUID);

            if (user is null)
                return new ProfileResponse()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                };

            var profile = await textures.GetTextures(user.Uuid.ToString(), user.Username, textures.GetSkin(user.Username), textures.GetCloak(user.Username));

            var result = new ProfileResponse()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = profile
            };

            return result;
        }

        public async Task<ProfilesResponse[]> ProfilesAsync(string[] users)
        {
            if (users == null || users.Length == 0)
                return Array.Empty<ProfilesResponse>();

            var profiles = Users.Where(x => users.Contains(x.Username)).Select(x => new ProfilesResponse() { Id = x.Uuid.ToString(), Name = x.Username }).ToArray();

            return profiles;
        }
    }
}
