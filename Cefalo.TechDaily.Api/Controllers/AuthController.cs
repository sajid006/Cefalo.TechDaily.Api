using Cefalo.TechDaily.Database.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Cefalo.TechDaily.Service.Dto;
using Cefalo.TechDaily.Service.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;

namespace Cefalo.TechDaily.Api.Controllers
{
    [Route("api/[controller]")]
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

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = _mapper.Map<User>(request);
            user.UpdatedAt = DateTime.Now;
            user.CreatedAt = DateTime.Now;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            var userDto = await _userService.PostUser(user);
            return CreatedAtAction(nameof(Signup), userDto.Id, userDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = _mapper.Map<User>(request);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            var userDto = await _authService.Login(user);
            return "Hello " + user.Username;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(User.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
