using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using NexteAPI.Interfaces;
using System.Threading.Tasks;

namespace NexteAPI.Controllers.Minecraft
{
    [Route("authlib/api/[controller]/[action]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        IAuthProvider provider;
        ILogger<PlayerController> logger;
        public PlayerController(IAuthProvider _provider, ILogger<PlayerController> _logger)
        {
            provider = _provider;
            logger = _logger;
        }

        [HttpGet]
        public async Task<IActionResult> Attributes()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];

            if (string.IsNullOrEmpty(accessToken))
                return ReturnBadRequest();

            var data = await provider.PrivilegesAsync(accessToken);

            if(data == null)
                return ReturnBadRequest();

            var json = JsonConvert.SerializeObject(data);
            return Ok(json);
        }

        private IActionResult ReturnBadRequest()
        {
            return BadRequest(new { error = "Bad login", errorMessage = "Bad login" });
        }
    }
}
