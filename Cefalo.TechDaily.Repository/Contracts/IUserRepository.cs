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
        Task<List<User>> GetUsers();
        Task<User?> GetUserByUsername(string Username);
        Task<User?> PostUser(User user);
        Task<User?> UpdateUser(string Username, User user);
        Task<Boolean> DeleteUser(string Username);

    }
}
