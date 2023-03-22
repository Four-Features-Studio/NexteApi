using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NexteAPI.DTO.AuthRequestes;
using NexteAPI.Interfaces;
using System;
using System.Threading.Tasks;

namespace NexteAPI.Controllers.Minecraft
{
    [Route("authlib/sessionserver/[controller]/minecraft/[action]")]
    [ApiController]
    public class SessionController : ControllerBase
    {

        IAuthProvider provider;
        ILogger<SessionController> logger;
        ITexturesService textures;
        public SessionController(IAuthProvider _provider, ILogger<SessionController> _logger, ITexturesService _textures)
        {
            provider = _provider;
            logger = _logger;
            textures = _textures;
        }

        [HttpPost]
        public async Task<IActionResult> Join([FromBody] JoinRequest join)
        {
            if (string.IsNullOrEmpty(join.SelectedProfile) || string.IsNullOrEmpty(join.AccessToken) || string.IsNullOrEmpty(join.ServerId))
            {
                return ReturnBadRequest();
            }

            var user = await provider.JoinAsync(join.AccessToken, join.SelectedProfile, join.ServerId);

            if (user is false)
            {
                return ReturnBadRequest();
            }

            return new NoContentResult();
        }

        [HttpGet]
        public async Task<IActionResult> HasJoined([FromQuery] string username, [FromQuery] string serverId)
        {
            var data = await provider.HasJoinedAsync(username, serverId);

            if (data is null || data.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ReturnBadRequest();
            }
            var json = JsonConvert.SerializeObject(data.Data);

            return Ok(json);
        }

        [HttpGet]
        [Route("{uuid}")]
        public async Task<IActionResult> Profile(string uuid, [FromQuery]bool unsigned = false)
        {
            var data = await provider.ProfileAsync(uuid);

            if(data is null || data.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ReturnBadRequest();
            }
            var json = JsonConvert.SerializeObject(data.Data);

            return Ok(json);
        }

        private IActionResult ReturnBadRequest()
        {
            return BadRequest(new { error = "Bad login", errorMessage = "Bad login" });
        }
    }
}
