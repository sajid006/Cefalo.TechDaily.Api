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
            _cookieHandler.Set("user", userWithToken.Token,100000);
            return Ok(userWithToken);
        }
        [HttpPost("verifytoken")]
        public async Task<ActionResult<string>> VerifyToken(TokenDto tokenDto)
        {
            var token = tokenDto.Token;
            if (token.IsNullOrEmpty()) return null;
            if (_jwtTokenHandler.HttpContextExists() == false) return null;
            var username = _jwtTokenHandler.VerifyToken(token);
            return username;
        }
        

    }
}
