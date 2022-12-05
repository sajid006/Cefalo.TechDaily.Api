using Cefalo.TechDaily.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Repository.Contracts
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
        Task<User?> GetUserByUsernameAsync(string Username);
        Task<int> CountUserByEmailAsync(string Email);
        Task<User?> PostUserAsync(User user);
        Task<User?> UpdateUserAsync(string Username, User user);
        Task<Boolean> DeleteUserAsync(string Username);

    }
}
