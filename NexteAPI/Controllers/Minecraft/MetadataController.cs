using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NexteAPI.Configs;
using NexteAPI.DTO;
using NexteAPI.Interfaces;
using System.Threading.Tasks;

namespace NexteAPI.Controllers.Minecraft
{
    [Route("authlib")]
    [ApiController]
    public class MetadataController : ControllerBase
    {
        //meta: {
        //        serverName:
        //            this.configManager.config.projectName || "Aurora Launcher",
        //        implementationName: "aurora-launchserver",
        //        implementationVersion: "0.0.1",
        //        "feature.no_mojang_namespace": true,
        //        "feature.privileges_api": true,
        //    },
        //    skinDomains: this.configManager.config.api.injector.skinDomains,
        //    signaturePublickey: this.authlibManager.getPublicKey(),

        IOptions<SystemConfig> options;
        IAuthlibService authlib;
        public MetadataController(IOptions<SystemConfig> _options, IAuthlibService _authlib)
        {
            options = _options;
            authlib = _authlib;
        }

        [HttpGet]
        public async Task<IActionResult> Metadata()
        {
            var meta = new MetadataResponse();
            meta.SkinDomains = options.Value.SkinDomains;
            meta.Meta.ServerName = options.Value.ProjectName;
            meta.Meta.ImplementationVersion = options.Value.ProjectVersion;

            meta.SignaturePublickey = await authlib.GetPublicKey();

            var json = JsonConvert.SerializeObject(meta);
            return Ok(json);
        }

    }
}
