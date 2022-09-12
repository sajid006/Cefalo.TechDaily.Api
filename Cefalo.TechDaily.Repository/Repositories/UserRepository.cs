using Cefalo.TechDaily.Database.Context;
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
        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User> GetUserByIdAsync(int Id)
        {
            var user = await _context.Users.FindAsync(Id);
            return user;
        }
        public async Task<User> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User?> UpdateUser(int Id, User user)
        {
            if (Id != user.Id) return null;
            var myUser = await _context.Users.FindAsync(Id);
            if (myUser == null) return null;
            myUser.Name = user.Name;
            myUser.Email = user.Email;
            myUser.CreatedAt = user.CreatedAt;
            myUser.UpdatedAt = user.UpdatedAt;
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<Boolean> DeleteUser(int Id)
        {
            var user = await _context.Users.FindAsync(Id);
            if(user == null) return false;
            _context.Users.Remove((User)user);
            await _context.SaveChangesAsync();
            return true;
        }

        

        

        

        
    }
}
