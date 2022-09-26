﻿using Cefalo.TechDaily.Database.Context;
using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User?> GetUserByUsername(string Username)
        {
            var userByName =  await _context.Users.FindAsync(Username);
            return userByName;
        }
        public async Task<User?> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User?> UpdateUser(string Username, User user)
        {
            var myUser = await _context.Users.FindAsync(Username);
            myUser.Email = user.Email;
            myUser.Name = user.Name;
            myUser.UpdatedAt = DateTime.UtcNow;
            if(user.PasswordHash != null)
            {
                myUser.PasswordHash = user.PasswordHash;
                myUser.PasswordSalt = user.PasswordSalt;
                myUser.PasswordModifiedAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
            return myUser;
        }
        public async Task<Boolean> DeleteUser(string Username)
        {
            var user = await _context.Users.FindAsync(Username);
            _context.Users.Remove((User)user);
            await _context.SaveChangesAsync();
            // if (user != null) return false;
            return true;
        }
    }
}
