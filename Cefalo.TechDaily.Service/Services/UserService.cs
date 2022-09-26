using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Repository.Contracts;
using Cefalo.TechDaily.Service.Contracts;
using Cefalo.TechDaily.Service.Dto;
using Cefalo.TechDaily.Service.Utils.Contract;
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

namespace Cefalo.TechDaily.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHandler _passwordHandler;
        private readonly IJwtTokenHandler _jwtTokenHandler;
        public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHandler passwordHandler, IJwtTokenHandler jwtTokenHandler)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHandler = passwordHandler;
            _jwtTokenHandler = jwtTokenHandler;
        }
        public async Task<List<UserDto>> GetUsers()
        {
            var users = await _userRepository.GetUsers();
            return users.Select(user => _mapper.Map<UserDto>(user)).ToList();
        }
        public async Task<UserDto> GetUserByUsername(string Username)
        {
            var user = await _userRepository.GetUserByUsername(Username);
            if (user == null) throw new NotFoundException("User not found");
            return _mapper.Map<UserDto>(user);
        }
        public async Task<UserDto> PostUser(SignupDto request)
        {
            _passwordHandler.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = _mapper.Map<User>(request);
            user.UpdatedAt = DateTime.UtcNow;
            user.CreatedAt = DateTime.UtcNow;
            user.PasswordModifiedAt = DateTime.UtcNow;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            var newUser = await _userRepository.PostUser(user);
            if (newUser == null) throw new BadRequestException("Cant add user");
            var userDto = _mapper.Map<UserDto>(newUser);
            return userDto;
        }

        public async Task<UserDto> UpdateUser(string Username, UpdateUserDto updateUserDto)
        {
            var currentUser = await _userRepository.GetUserByUsername(Username);
            if (currentUser == null) throw new NotFoundException("User not found");
            var loggedInUser = _jwtTokenHandler.GetLoggedinUsername();
            if (loggedInUser != Username) throw new UnauthorizedException("You are not authorized to update this user's information");
            var tokenCreationTimeString = _jwtTokenHandler.GetTokenCreationTime();
            if (tokenCreationTimeString == null) throw new UnauthorizedException("Please login again");
            DateTime tokenCreationTime = Convert.ToDateTime(tokenCreationTimeString); 
            if(DateTime.Compare(tokenCreationTime,currentUser.PasswordModifiedAt)<0) throw new UnauthorizedException("Please login again");
            User user = _mapper.Map<User>(updateUserDto);
            if (updateUserDto.Password != null)
            {
                _passwordHandler.CreatePasswordHash(updateUserDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.PasswordModifiedAt = DateTime.UtcNow;
            }
            user.UpdatedAt = DateTime.UtcNow;
            var newUser = await _userRepository.UpdateUser(Username, user);
            var userDto = _mapper.Map<UserDto>(newUser);
            return userDto;
        }
        public async Task<Boolean> DeleteUser(string Username)
        {
            var currentUser = await _userRepository.GetUserByUsername(Username);
            if (currentUser == null) throw new NotFoundException("User not found");
            var loggedInUser = _jwtTokenHandler.GetLoggedinUsername();
            if (loggedInUser != Username) throw new UnauthorizedException("You are not authorized to delete this user");
            var tokenCreationTimeString = _jwtTokenHandler.GetTokenCreationTime();
            if (tokenCreationTimeString == null) throw new UnauthorizedException("Please login again");
            DateTime tokenCreationTime = Convert.ToDateTime(tokenCreationTimeString);
            if (DateTime.Compare(tokenCreationTime, currentUser.PasswordModifiedAt) < 0) throw new UnauthorizedException("Please login again");

            return await _userRepository.DeleteUser(Username);
        }

        

        

        
    }
}
