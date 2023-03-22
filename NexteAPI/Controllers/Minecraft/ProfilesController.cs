using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NexteAPI.Entity;
using NexteAPI.Interfaces;
using System.Threading.Tasks;

namespace NexteAPI.Controllers.Minecraft
{
    [Route("authlib/api/[controller]/[action]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        IAuthProvider provider;
        ILogger<ProfilesController> logger;
        public ProfilesController(IAuthProvider _provider, ILogger<ProfilesController> _logger)
        {
            provider = _provider;
            logger = _logger;
        }

        [HttpPost]
        public async Task<IActionResult> Minecraft([FromBody] string[] users)
        {
            if(users is null)
            {
                ReturnBadRequest();
            }

            if (users.Length == 0)
            {
                return NoContent();
            }

            var data = await provider.ProfilesAsync(users);

            if (data is null)
            {
                ReturnBadRequest();
            }

            if (data.Length == 0)
            {
                return NoContent();
            }

            var json = JsonConvert.SerializeObject(data);

            return Ok(json);
        }

        private IActionResult ReturnBadRequest()
        {
            return BadRequest(new { error = "Bad login", errorMessage = "Bad login" });
        }
    }
}
