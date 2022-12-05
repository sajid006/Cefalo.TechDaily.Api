using AutoMapper;
using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Repository.Contracts;
using Cefalo.TechDaily.Service.Contracts;
using Cefalo.TechDaily.Service.CustomExceptions;
using Cefalo.TechDaily.Service.Dto;
using Cefalo.TechDaily.Service.DtoValidators;
using Cefalo.TechDaily.Service.Utils.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private readonly BaseDtoValidator<LoginDto> _loginDtoValidator;
        private readonly BaseDtoValidator<SignupDto> _signupDtoValidator;

        public AuthService(IUserRepository userRepository, IMapper mapper, IPasswordHandler passwordHandler, IJwtTokenHandler jwtTokenHandler, BaseDtoValidator<LoginDto> loginDtoValidator, BaseDtoValidator<SignupDto> signupDtoValidator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHandler = passwordHandler;
            _jwtTokenHandler = jwtTokenHandler;
            _loginDtoValidator = loginDtoValidator;
            _signupDtoValidator = signupDtoValidator;
        }
        public async Task<UserWithToken> SignupAsync(SignupDto request)
        {
            _signupDtoValidator.ValidateDTO(request);
            var userByUsername = await _userRepository.GetUserByUsernameAsync(request.Username);
            if (userByUsername != null) throw new BadRequestException("Username must be unique");
            var CountUserByEmail = await _userRepository.CountUserByEmailAsync(request.Email);
            if (CountUserByEmail > 0) throw new BadRequestException("Email must be unique");
            Tuple<byte[], byte[]> passwordObject = _passwordHandler.CreatePasswordHash(request.Password);
            byte[] passwordSalt = passwordObject.Item1;
            byte[] passwordHash = passwordObject.Item2;
            var user = _mapper.Map<User>(request);
            user.UpdatedAt = DateTime.UtcNow;
            user.CreatedAt = DateTime.UtcNow;
            user.PasswordModifiedAt = DateTime.UtcNow;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            var newUser = await _userRepository.PostUserAsync(user);
            var userWithToken = _mapper.Map<UserWithToken>(newUser);
            userWithToken.Token = _jwtTokenHandler.CreateToken(newUser);
            return userWithToken;
        }
        public async Task<UserWithToken> LoginAsync(LoginDto request)
        {
            _loginDtoValidator.ValidateDTO(request);
            var user = await _userRepository.GetUserByUsernameAsync(request.Username);
            if (user == null) throw new BadRequestException("Invalid username or password");
            bool isPasswordCorrect = _passwordHandler.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);
            if (!isPasswordCorrect) throw new BadRequestException("Invalid username or password");
            var userWithToken = _mapper.Map<UserWithToken>(user);
            userWithToken.Token = _jwtTokenHandler.CreateToken(user);
            
            return userWithToken;
        }

        public async Task<string?> GetCurrentUserAsync()
        {
            if (_jwtTokenHandler.HttpContextExists() == false) return null;
            var username = _jwtTokenHandler.GetLoggedinUsername();
            //var user = await _userRepository.GetUserByUsernameAsync(username);
            //if (user == null) return null;
            return username;
        }
        public async void LogoutAsync()
        {
            _jwtTokenHandler.DeleteToken();
        }

    }
}
