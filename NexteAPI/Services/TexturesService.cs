using NexteAPI.Entity;
using NexteAPI.Interfaces;
using Org.BouncyCastle.Asn1.X509;
using System.Collections.Generic;
using System;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using NexteAPI.Configs;
using System.Security.Cryptography;

namespace NexteAPI.Services
{
    public class TexturesService : ITexturesService
    {
        IAuthlibService authlib;
        IWebHostEnvironment appEnvironment;
        SystemConfig config;
        string pathSkin;
        string pathCloak;

        public TexturesService(IAuthlibService _authlib, IWebHostEnvironment _appEnvironment, IOptions<SystemConfig> _config)
        {
            authlib = _authlib;
            appEnvironment = _appEnvironment;
            config = _config.Value;
            pathSkin = Path.Combine(_appEnvironment.ContentRootPath, config.TexturesOptions.PathSkin);
            pathCloak = Path.Combine(_appEnvironment.ContentRootPath, config.TexturesOptions.PathCloak);

            if (!Directory.Exists(pathSkin + "cache"))
            {
                Directory.CreateDirectory(pathSkin + "cache");
            }
            if (!Directory.Exists(pathCloak + "cache"))
            {
                Directory.CreateDirectory(pathCloak + "cache");
            }
        }

        public string GetSkin(string username)
        {
            var fileName = username + ".png";
            var pathToSkinUser = Path.Combine(pathSkin, fileName);
            var skinUrl = string.Empty;
            if (File.Exists(pathToSkinUser))
            {
                var pathToSkinInCache = Path.Combine(pathSkin, "cache\\" + username + ".png");
                File.Copy(pathToSkinUser, pathToSkinInCache, true);
                skinUrl = config.DomainSite + "MinecraftSkins/" + username + ".png";
            }
            else if (config.TexturesOptions.DefaultSkinEnabled)
            {
                pathToSkinUser = Path.Combine(pathSkin, config.TexturesOptions.DefaultSkinName);
                var pathToSkinInCache = Path.Combine(pathSkin, "cache\\" + username + ".png");
                File.Copy(pathToSkinUser, pathToSkinInCache, true);
                skinUrl = config.DomainSite + "MinecraftSkins/" + username + ".png";
            }
            return skinUrl;
        }

        public string GetCloak(string username)
        {

            var fileName = username + ".png";
            var pathToCloakUser = Path.Combine(pathCloak, fileName);
            var cloakUrl = string.Empty;
            if (File.Exists(pathToCloakUser))
            {
                var pathToCloakInCache = Path.Combine(pathCloak, "cache\\" + username + ".png");
                File.Copy(pathToCloakUser, pathToCloakInCache, true);
                cloakUrl = config.DomainSite + "MinecraftCloaks/" + username + ".png";
            }
            else if (config.TexturesOptions.DefaultCloakEnabled)
            {
                pathToCloakUser = Path.Combine(pathCloak, config.TexturesOptions.DefaultCloakName);
                var pathToCloakInCache = Path.Combine(pathCloak, "cache\\" + username + ".png");
                File.Copy(pathToCloakUser, pathToCloakInCache, true);
                cloakUrl = config.DomainSite + "MinecraftCloaks/" + username + ".png";
            }
            return cloakUrl;
        }

        public async Task<Profile> GetTextures(string uuidProfile, string username, string skinUrl, string cloakUrl)
        {
            var profile = new Profile()
            {
                Id = uuidProfile,
                Name = username,
                Properties = new List<ProfileProperties>()
            };
            var texture = new PropertyTextures()
            {
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ProfileName = username,
                ProfileId = uuidProfile,
                Textures = new Textures()
                {
                    Cape = new SkinCape()
                    {
                        Url = cloakUrl
                    },
                    Skin = new SkinCape()
                    {
                        Url = skinUrl
                    }
                }
            };
            var base64Value = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(texture)));
            var signature = await authlib.GetSignature(base64Value);
            profile.Properties.Add(new ProfileProperties()
            {
                Value = base64Value,
                Signature = signature
            });

            return profile;
        }

        private string GetMD5HashFromString(string str)
        {
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            return BitConverter.ToString(hash).Replace("-", "");
        }
    }
}
