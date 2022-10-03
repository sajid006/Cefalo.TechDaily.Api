using Cefalo.TechDaily.Database.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Cefalo.TechDaily.Service.Dto;
using Cefalo.TechDaily.Service.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.AspNetCore.Authorization;
using Cefalo.TechDaily.Service.Services;
using Cefalo.TechDaily.Service.Utils.Contracts;
using Cefalo.TechDaily.Service.CustomExceptions;
using Microsoft.IdentityModel.Tokens;

namespace Cefalo.TechDaily.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICookieHandler _cookieHandler;
        private readonly IJwtTokenHandler _jwtTokenHandler;
        public AuthController(IAuthService authService,IJwtTokenHandler jwtTokenHandler, ICookieHandler cookieHandler)
        {
            _authService = authService;
            _cookieHandler = cookieHandler;
            _jwtTokenHandler = jwtTokenHandler;
        }
        [HttpPost("signup")]
        public async Task<ActionResult<UserWithToken>> Signup(SignupDto request)
        {
            var userWithToken = await _authService.Signup(request);
            return CreatedAtAction(nameof(Signup), userWithToken);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserWithToken>> Login(LoginDto request)
        {
            var userWithToken = await _authService.Login(request);
            //_cookieHandler.Set("user", userWithToken.Token,100000);
            return Ok(userWithToken);
        }
        [HttpPost("verifytoken"),Authorize]
        public async Task<ActionResult<string?>> Verify()
        {
            
            var loggedInUser = await _authService.GetCurrentUser();
            if (loggedInUser == null) return null;
            return loggedInUser.ToString();
            
        }
        [HttpGet("logout")]
        public async Task<ActionResult> Logout()
        {
            _authService.Logout();
            return Ok();
        }
        

    }
}
