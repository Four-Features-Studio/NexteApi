using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NexteAPI.Configs;
using NexteAPI.DTO.AuthResponses;
using NexteAPI.EFCore;
using NexteAPI.Entity;
using NexteAPI.Interfaces;
using NexteServer.Efcore;
using Org.BouncyCastle.Bcpg.Sig;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexteAPI.Services.Providers
{
    public class DataBaseAuthProvider : IAuthProvider
    {
        SystemConfig options;
        IServiceScopeFactory scopeFactory;
        ITexturesService textures;
        public DataBaseAuthProvider(IOptions<SystemConfig> _options, IServiceScopeFactory _scopeFactory, ITexturesService _textures)
        {
            scopeFactory = _scopeFactory;
            options = _options.Value;
            textures = _textures;
        }

        public TypeAuthProvider Type { get; } = TypeAuthProvider.Database;

        public async Task<AuthLoginResponse> LoginAsync(string username, string password)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<NexteDbContext>();

                string hash_password = string.Empty;

                switch (options.AuthProvider.PasswordHashMethod)
                {
                    case TypePasswordHash.SHA1:
                        {
                            var data = Encoding.UTF8.GetBytes(password);
                            hash_password = Hasher.ComputeSHA1(data);
                        }
                        break;
                    case TypePasswordHash.BCrypt:
                        {
                            hash_password = BCrypt.Net.BCrypt.HashPassword(password);
                        }
                        break;
                    case TypePasswordHash.MD5:
                        break;
                    case TypePasswordHash.DoubleMD5:
                        break;
                }

                if (await db.Users.AnyAsync(x => x.Username == username))
                {
                    var user = await db.Users.FirstOrDefaultAsync(x => x.Username == username && x.Password == hash_password);

                    if (user is null)
                        return new AuthLoginResponse() { Successful = false, Message = "Неверный логин или пароль" };

                    user.AccessToken = Guid.NewGuid().ToString();
                    await db.SaveChangesAsync();

                    var result = new AuthLoginResponse()
                    {
                        Successful = true,
                        Profile = new UserProfile()
                        {
                            AccessToken = user.AccessToken,
                            Username = user.Username,
                            Uuid = user.Uuid.ToString(),
                            Avatar = user.Avatar
                        }
                    };

                    return result;

                }

                return new AuthLoginResponse() { Successful = false, Message = "Неверный логин или пароль" };
            }
        }
        public async Task LogoutAsync(string accessToken)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<NexteDbContext>();
                var user = await db.Users.FirstOrDefaultAsync(x => x.AccessToken == accessToken);

                if (user is null)
                    return;

                user.AccessToken = String.Empty;
                await db.SaveChangesAsync();
            }
        }

        public async Task<HasJoinedResponse> HasJoinedAsync(string username, string serverId)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<NexteDbContext>();
                var user = await db.Users.FirstOrDefaultAsync(x => x.Username == username && x.ServerId == serverId);

                if (user is null)
                    return new HasJoinedResponse()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                    };

                var profile = await textures.GetTextures(user.Uuid.ToString(), user.Username, user.SkinUrl, user.CloakUrl);
                var result = new HasJoinedResponse()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = profile
                };
                return result;
            }
        }

        public async Task<bool> JoinAsync(string accessToken, string userUUID, string serverId)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<NexteDbContext>();
                var user = await db.Users.FirstOrDefaultAsync(x => x.Uuid.ToString() == userUUID && x.AccessToken == accessToken);

                if (user is null)
                    return false;

                user.ServerId = serverId;
                await db.SaveChangesAsync();

                return true;
            }
        }

        public async Task<PrivilegesResponse> PrivilegesAsync(string userUUID)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<NexteDbContext>();
                var user = await db.Users.FirstOrDefaultAsync(x => x.Uuid.ToString() == userUUID);

                if (user is null)
                    return null;

                var result = new PrivilegesResponse()
                {
                    OnlineChat = user.OnlineChat,
                    MultiplayerServer = user.MultiplayerServer,
                    MultiplayerRealms = user.MultiplayerRealms,
                    Telemetry = user.Telemetry
                };

                return result;
            }
        }

        public async Task<ProfileResponse> ProfileAsync(string userUUID)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<NexteDbContext>();

                var user = await db.Users.FirstOrDefaultAsync(x => x.Uuid.ToString() == userUUID);

                if (user is null)
                    return new ProfileResponse()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                    };

                var profile = await textures.GetTextures(user.Uuid.ToString(), user.Username, user.SkinUrl, user.CloakUrl);
                var result = new ProfileResponse()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = profile
                };

                return result;
            }
        }

        public async Task<ProfilesResponse[]> ProfilesAsync(string[] users)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<NexteDbContext>();

                if (users == null || users.Length == 0)
                    return Array.Empty<ProfilesResponse>();

                var profiles = await db.Users.Where(x => users.Contains(x.Username)).Select(x => new ProfilesResponse() { Id = x.Uuid.ToString(), Name = x.Username }).ToArrayAsync();

                return profiles;
            }

        }
    }
}
