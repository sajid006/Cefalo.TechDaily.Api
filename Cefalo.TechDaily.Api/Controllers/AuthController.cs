using Cefalo.TechDaily.Database.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Cefalo.TechDaily.Service.Dto;
using Cefalo.TechDaily.Service.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;

namespace Cefalo.TechDaily.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public AuthController(IUserService userService, IMapper mapper, IAuthService authService)
        {
            _userService = userService;
            _mapper = mapper;
            _authService = authService;
        }
        [HttpPost("signup")]
        public async Task<ActionResult<UserDto>> Signup(SignupDto request)
        {

            var userDto = await _authService.Signup(request);
            if (userDto == null) return BadRequest("Cant create user");
            return CreatedAtAction(nameof(Signup), userDto.Id, userDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto request)
        {
            var token = await _authService.Login(request);
            if (token == null) return BadRequest("Invalid Username or Password");
            return token;
        }
        
    }
}
