using Cefalo.TechDaily.Database.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Cefalo.TechDaily.Service.Dto;
using Cefalo.TechDaily.Service.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.AspNetCore.Authorization;
using Cefalo.TechDaily.Service.Services;

namespace Cefalo.TechDaily.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthController(IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
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
            Response.Cookies.Append("user", userWithToken.Token);
            return Ok(Response);
        }
        [HttpPost("verifytoken")]
        public async Task<string> VerifyToken()
        {
            return await _authService.GetCurrentUser();
        }

    }
}
