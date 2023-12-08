using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using testApi.Models;
using testApi.Service;
using testApi.Interfaces;
using testApi.Entities;

using AuthorizeAttribute = testApi.Models.AuthorizeAttribute;

namespace testApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userService.Login(model.Username, model.Password);

            if (user == null)  
            {
                return NotFound("Kullanıcı adı veya şifre yanlış!");
            }

            user.Password = "****";

            return Ok(user);
        }

        [HttpGet("authTest")]
        [Authorize]
        public async Task<IActionResult> authTest()
        {
            return Ok( "authTest");
        }


        [HttpGet("getUsers")]
        [Authorize]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();
            return Ok(users);
        }

        [HttpGet("getUserById")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserById(id);


            if (user == null)
            {
                return NotFound("Kullanıcı Bulunamadı");
            }

            return Ok(user);
        }
    }
}
