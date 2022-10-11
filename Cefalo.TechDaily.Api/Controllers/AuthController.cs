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

        private readonly IJwtTokenHandler _jwtTokenHandler;
        public AuthController(IAuthService authService,IJwtTokenHandler jwtTokenHandler)
        {
            _authService = authService;
            _jwtTokenHandler = jwtTokenHandler;
        }


        [HttpPost("signup")]
        public async Task<ActionResult<UserWithToken>> SignupAsync(SignupDto request)
        {
            var userWithToken = await _authService.SignupAsync(request);
            return CreatedAtAction(nameof(SignupAsync), userWithToken);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserWithToken>> LoginAsync(LoginDto request)
        {
            var userWithToken = await _authService.LoginAsync(request);
            //_cookieHandler.Set("user", userWithToken.Token,100000);
            return Ok(userWithToken);
        }
        [HttpPost("verifytoken")]
        public async Task<ActionResult<string?>> VerifyAsync()
        {
            var loggedInUser = await _authService.GetCurrentUserAsync();
            if (loggedInUser == null) return null;
            return loggedInUser.ToString();
        }
        [HttpGet("logout")]
        public async Task<ActionResult> LogoutAsync()
        {
            _authService.LogoutAsync();
            return Ok();
        }
        

    }
}
