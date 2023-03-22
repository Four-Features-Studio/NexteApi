using NexteAPI.Entity;
using NexteAPI.Interfaces;
using Org.BouncyCastle.Asn1.X509;
using System.Collections.Generic;
using System;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace NexteAPI.Services
{
    public class TexturesService : ITexturesService
    {
        IAuthlibService authlib;
        public TexturesService(IAuthlibService _authlib)
        {
            authlib = _authlib;
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
    }
}
