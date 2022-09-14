﻿using Cefalo.TechDaily.Database.Context;
using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Repository.Repositories;
using Cefalo.TechDaily.Repository.Contracts;
using Cefalo.TechDaily.Service.Contracts;
using Cefalo.TechDaily.Service.Dto;
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
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<List<User>> GetUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }
        public async Task<User?> GetUserByIdAsync(int Id)
        {
            return await _userRepository.GetUserByIdAsync(Id);
        }
        public async Task<UserDto> PostUser(User user)
        {
            User newUser = await _userRepository.PostUser(user);
            var userDto = new UserDto
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Email = newUser.Email,
                Name = newUser.Name,
                CreatedAt = newUser.CreatedAt,
                UpdatedAt = newUser.UpdatedAt
            };
            return userDto;
        }

        public async Task<UserDto?> UpdateUser(int Id, User user)
        {
            if (Id != user.Id) return null;
            var newUser = await _userRepository.UpdateUser(Id, user);
            if (newUser == null) return null;
            var userDto = new UserDto
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Email = newUser.Email,
                Name = newUser.Name,
                CreatedAt = newUser.CreatedAt,
                UpdatedAt = newUser.UpdatedAt
            };
            return userDto;
        }
        public Task<Boolean> DeleteUser(int Id)
        {
            return _userRepository.DeleteUser(Id);
        }

        

        

        
    }
}