using Cefalo.TechDaily.Database.Context;
using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Repository.Repositories;
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

namespace Cefalo.TechDaily.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<List<UserDto>> GetUsersAsync()
        {
            var users = await _userRepository.GetUsersAsync();
            return users.Select(user => _mapper.Map<UserDto>(user)).ToList();
        }
        public async Task<UserDto?> GetUserByIdAsync(int Id)
        {
            var user =  await _userRepository.GetUserByIdAsync(Id);
            return _mapper.Map<UserDto>(user);
        }
        public async Task<UserDto?> GetUserByUsernameAsync(string Username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(Username);
            return _mapper.Map<UserDto>(user);
        }
        public async Task<UserDto> PostUser(User user)
        {
            User newUser = await _userRepository.PostUser(user);
            var userDto = _mapper.Map<UserDto>(newUser);
            return userDto;
        }

        public async Task<UserDto?> UpdateUser(int Id, UpdateDto updateDto)
        {
            if (Id != updateDto.Id) return null;
            User user = _mapper.Map<User>(updateDto);
            var newUser = await _userRepository.UpdateUser(Id, user);
            if (newUser == null) return null;
            var userDto = _mapper.Map<UserDto>(newUser);
            return userDto;
        }
        public Task<Boolean> DeleteUser(int Id)
        {
            return _userRepository.DeleteUser(Id);
        }

        

        

        
    }
}
