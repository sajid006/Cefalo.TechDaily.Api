using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Repository.Contracts;
using Cefalo.TechDaily.Service.Contracts;
using Cefalo.TechDaily.Service.Dto;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Runtime.ConstrainedExecution;
using Cefalo.TechDaily.Service.Utils.Contracts;
using Cefalo.TechDaily.Service.CustomExceptions;
using FluentValidation;
using Cefalo.TechDaily.Service.DtoValidators;

namespace Cefalo.TechDaily.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHandler _passwordHandler;
        private readonly IJwtTokenHandler _jwtTokenHandler;
        private readonly BaseDtoValidator<SignupDto> _signupDtoValidator;
        private readonly BaseDtoValidator<UpdateUserDto> _updateUserDtoValidator;
        public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHandler passwordHandler, IJwtTokenHandler jwtTokenHandler, BaseDtoValidator<SignupDto> signupDtoValidator, BaseDtoValidator<UpdateUserDto> updateUserDtoValidator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHandler = passwordHandler;
            _jwtTokenHandler = jwtTokenHandler;
            _signupDtoValidator = signupDtoValidator;
            _updateUserDtoValidator = updateUserDtoValidator;
        }
        public async Task<List<UserDto>> GetUsersAsync()
        {
            var users = await _userRepository.GetUsersAsync();
            var userList =  users.Select(user => _mapper.Map<UserDto>(user)).ToList();
            return userList;
        }
        public async Task<UserDto> GetUserByUsernameAsync(string Username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(Username);
            if (user == null) throw new NotFoundException("User not found");
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }
        public async Task<UserWithToken> PostUserAsync(SignupDto request)
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

        public async Task<UserDto> UpdateUserAsync(string Username, UpdateUserDto updateUserDto)
        {
            _updateUserDtoValidator.ValidateDTO(updateUserDto);
            var currentUser = await _userRepository.GetUserByUsernameAsync(Username);
            if (currentUser == null) throw new NotFoundException("User not found");
            var loggedInUser = _jwtTokenHandler.GetLoggedinUsername();
            if (loggedInUser != Username) throw new UnauthorizedException("You are not authorized to update this user's information");
            var tokenCreationTimeString = _jwtTokenHandler.GetTokenCreationTime();
            if (tokenCreationTimeString == null) throw new UnauthorizedException("Please login again");
            DateTime tokenCreationTime = Convert.ToDateTime(tokenCreationTimeString); 
            if(DateTime.Compare(tokenCreationTime,currentUser.PasswordModifiedAt)<0) throw new UnauthorizedException("Please Login Again");
            User user = _mapper.Map<User>(updateUserDto);
            if (updateUserDto.Password != null && updateUserDto.Password != "")
            {
                if (updateUserDto.Password.Length < 8) throw new BadRequestException("Password length must be at least 8");
                Tuple<byte[], byte[]> passwordObject = _passwordHandler.CreatePasswordHash(updateUserDto.Password);
                byte[] passwordSalt = passwordObject.Item1;
                byte[] passwordHash = passwordObject.Item2;
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.PasswordModifiedAt = DateTime.UtcNow;
            }
            user.UpdatedAt = DateTime.UtcNow;
            var newUser = await _userRepository.UpdateUserAsync(Username, user);
            var userDto = _mapper.Map<UserDto>(newUser);
            return userDto;
        }
        public async Task<Boolean> DeleteUserAsync(string Username)
        {
            var currentUser = await _userRepository.GetUserByUsernameAsync(Username);
            if (currentUser == null) throw new NotFoundException("User not found");
            var loggedInUser = _jwtTokenHandler.GetLoggedinUsername();
            if (loggedInUser != Username) throw new UnauthorizedException("You are not authorized to delete this user");
            var tokenCreationTimeString = _jwtTokenHandler.GetTokenCreationTime();
            if (tokenCreationTimeString == null) throw new UnauthorizedException("Please login again");
            DateTime tokenCreationTime = Convert.ToDateTime(tokenCreationTimeString);
            if (DateTime.Compare(tokenCreationTime, currentUser.PasswordModifiedAt) < 0) throw new UnauthorizedException("Please Login Again");
            return await _userRepository.DeleteUserAsync(Username);
        }

        

        

        
    }
}
