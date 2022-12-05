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
        public async Task<IActionResult> SignupAsync(SignupDto request)
        {
            var userWithToken = await _authService.SignupAsync(request);
            if (userWithToken == null) return BadRequest("Can't create user");
            return Created(nameof(SignupAsync), userWithToken);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto request)
        {
            var userWithToken = await _authService.LoginAsync(request);
            if (userWithToken == null) return BadRequest("Failed To Login");
            return Ok(userWithToken);
        }
        [HttpPost("verifytoken")]
        public async Task<IActionResult> VerifyAsync()
        {
            var loggedInUser = await _authService.GetCurrentUserAsync();
            return Ok(loggedInUser);
        }
        /*
        [HttpGet("logout")]
        public async Task<ActionResult> LogoutAsync()
        {
            _authService.LogoutAsync();
            return Ok();
        }
        */

    }
}
