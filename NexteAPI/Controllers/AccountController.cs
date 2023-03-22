using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexteAPI.DTO.AuthRequestes;
using NexteAPI.DTO.AuthResponses;
using NexteAPI.Entity;
using NexteAPI.Interfaces;
using NexteServer.Efcore;
using System;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace NexteAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        IAuthProvider provider;
        public AccountController(IAuthProvider _provider)
        {
            provider = _provider;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]AuthLoginRequest authLogin)
        {
            var profile = await provider.LoginAsync(authLogin.Username, authLogin.Password);

            if (profile is null)
                return BadRequest(new AuthLoginResponse()
                {
                    Successful = true,
                    Message = "Не верный логин или пароль"
                });

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

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Logout([FromBody]AuthLogoutRequest authLogin)
        {
            await provider.LogoutAsync(authLogin.AccessToken);

            return Ok();
        }
    }
}
