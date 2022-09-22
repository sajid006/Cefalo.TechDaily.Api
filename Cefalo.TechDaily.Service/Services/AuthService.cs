using AutoMapper;
using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Repository.Contracts;
using Cefalo.TechDaily.Service.Contracts;
using Cefalo.TechDaily.Service.Dto;
using Cefalo.TechDaily.Service.Utils.Contract;
using Cefalo.TechDaily.Service.Utils.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHandler _passwordHandler;
        private readonly IJwtTokenHandler _jwtTokenHandler;
        public AuthService(IUserRepository userRepository, IMapper mapper, IPasswordHandler passwordHandler, IJwtTokenHandler jwtTokenHandler)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHandler = passwordHandler;
            _jwtTokenHandler = jwtTokenHandler;
        }
        public async Task<UserDto?> Signup(SignupDto request)
        {
            _passwordHandler.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = _mapper.Map<User>(request);
            user.UpdatedAt = DateTime.UtcNow;
            user.CreatedAt = DateTime.UtcNow;
            user.PasswordModifiedAt = DateTime.UtcNow;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            var newUser = await _userRepository.PostUser(user);
            var userDto = _mapper.Map<UserDto>(newUser);
            return userDto;
        }
        public async Task<string? > Login(LoginDto request)
        {
            var user = await _userRepository.GetUserByUsername(request.Username);
            if (user == null) return null;
            bool isPasswordCorrect = _passwordHandler.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);
            if (!isPasswordCorrect) return null;
            string token = _jwtTokenHandler.CreateToken(user);
            return token;
        }

        public Task<UserDto?> Logout()
        {
            throw new NotImplementedException();
        }

    }
}
