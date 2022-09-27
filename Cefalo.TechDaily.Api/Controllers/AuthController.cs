using Cefalo.TechDaily.Database.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Cefalo.TechDaily.Service.Dto;
using Cefalo.TechDaily.Service.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.AspNetCore.Authorization;

namespace Cefalo.TechDaily.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
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
            return userWithToken;
        }

    }
}
