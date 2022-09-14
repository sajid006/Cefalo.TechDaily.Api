﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Service.Dto;
namespace Cefalo.TechDaily.Service.Contracts
{
    public interface IUserService
    {
        Task<List<User>> GetUsersAsync();
        Task<User?> GetUserByIdAsync(int Id);
        Task<UserDto> PostUser(User user);
        Task<UserDto?> UpdateUser(int Id, User user);
        Task<Boolean> DeleteUser(int Id);
    }
}