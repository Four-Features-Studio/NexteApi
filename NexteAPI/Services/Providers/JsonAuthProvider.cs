using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using NexteAPI.Configs;
using NexteAPI.DTO.AuthRequestes;
using NexteAPI.DTO.AuthResponses;
using NexteAPI.EFCore;
using NexteAPI.Entity;
using NexteAPI.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace NexteAPI.Services.Providers
{
    public class JsonAuthProvider : IAuthProvider
    {
        IOptions<SystemConfig> options;
        IWebRequestService webRequest;
        public JsonAuthProvider(IOptions<SystemConfig> _options, IWebRequestService _webRequest)
        {
            options = _options;
            webRequest = _webRequest;
        }

        public TypeAuthProvider Type { get; } = TypeAuthProvider.Json;

        public async Task<AuthLoginResponse> LoginAsync(string username, string password)
        {
            var url = UrlCombiner.Combine(options.Value.AuthProvider.JsonUrl, options.Value.AuthProvider.JsonActionsUrl.Login);

            var response = await webRequest.POST(url, new { username = username, password = password });

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var profile = JsonConvert.DeserializeObject<UserProfile>(content);

                var result = new AuthLoginResponse()
                {
                    Successful = true,
                    Profile = new UserProfile()
                    {
                        AccessToken = profile.AccessToken,
                        Username = profile.Username,
                        Uuid = profile.Uuid,
                        Avatar = profile.Avatar
                    }
                };

                return result;
            }

            return new AuthLoginResponse() { Successful = false, Message = "Неверный логин или пароль" };
        }

        public async Task LogoutAsync(string accessToken)
        {
            var url = UrlCombiner.Combine(options.Value.AuthProvider.JsonUrl, options.Value.AuthProvider.JsonActionsUrl.Logout);
            await webRequest.POST(url, new { accessToken = accessToken});
        }


        public async Task<HasJoinedResponse> HasJoinedAsync(string username, string serverId)
        {
            var url = UrlCombiner.Combine(options.Value.AuthProvider.JsonUrl, options.Value.AuthProvider.JsonActionsUrl.HasJoined);

            var response = await webRequest.GET(url, new Dictionary<string, string> { ["username"] = username, ["serverId"] = serverId });

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var profile = JsonConvert.DeserializeObject<Profile>(content);

                var result = new HasJoinedResponse()
                {
                    StatusCode = response.StatusCode,
                    Data = profile
                };

                return result;
            }

            return new HasJoinedResponse()
            {
                StatusCode = response.StatusCode,
            };
        }

        public async Task<bool> JoinAsync(string accessToken, string userUUID, string serverId)
        {
            var url = UrlCombiner.Combine(options.Value.AuthProvider.JsonUrl, options.Value.AuthProvider.JsonActionsUrl.Join);

            var response = await webRequest.POST(url, new { accessToken = accessToken, selectedProfile = userUUID, serverId = serverId });

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<PrivilegesResponse> PrivilegesAsync(string userUUID)
        {
            var url = UrlCombiner.Combine(options.Value.AuthProvider.JsonUrl, options.Value.AuthProvider.JsonActionsUrl.Profiles);

            var headers = new Dictionary<string, string>();
            headers.Add(HeaderNames.Authorization, $"Bearer {userUUID}");

            var response = await webRequest.GET(url, customHeaders: headers);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var attributes = JsonConvert.DeserializeObject<PrivilegesResponse>(content);
                return attributes;
            }

            return null;
        }

        public async Task<ProfileResponse> ProfileAsync(string userUUID)
        {
            var url = UrlCombiner.Combine(options.Value.AuthProvider.JsonUrl, options.Value.AuthProvider.JsonActionsUrl.Profile, userUUID);

            var response = await webRequest.GET(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var profile = JsonConvert.DeserializeObject<Profile>(content);

                var result = new ProfileResponse()
                {
                    StatusCode = response.StatusCode,
                    Data = profile
                };

                return result;
            }

            return new ProfileResponse()
            {
                StatusCode = response.StatusCode,
            };
        }

        public async Task<ProfilesResponse[]> ProfilesAsync(string[] users)
        {
            var url = UrlCombiner.Combine(options.Value.AuthProvider.JsonUrl, options.Value.AuthProvider.JsonActionsUrl.Profiles);

            var response = await webRequest.POST(url, users);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var profile = JsonConvert.DeserializeObject<ProfilesResponse[]>(content);
                return profile;
            }

            return null;
        }
    }
}
