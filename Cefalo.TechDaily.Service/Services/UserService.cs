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


namespace Cefalo.TechDaily.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHandler _passwordHandler;
        public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHandler passwordHandler)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHandler = passwordHandler;
        }
        public async Task<List<UserDto>> GetUsers()
        {
            var users = await _userRepository.GetUsers();
            return users.Select(user => _mapper.Map<UserDto>(user)).ToList();
        }
        public async Task<UserDto?> GetUserByUsername(string Username)
        {
            var user = await _userRepository.GetUserByUsername(Username);
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
            var userDto = _mapper.Map<UserDto>(newUser);
            return userDto;
        }

        public async Task<UserDto?> UpdateUser(string Username, UpdateUserDto updateUserDto)
        {

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
            if (newUser == null) return null;
            var userDto = _mapper.Map<UserDto>(newUser);
            return userDto;
        }
        public Task<Boolean> DeleteUser(string Username)
        {
            return _userRepository.DeleteUser(Username);
        }

        

        

        
    }
}
