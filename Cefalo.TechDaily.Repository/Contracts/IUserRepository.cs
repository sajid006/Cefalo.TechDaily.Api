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
        Task<User> GetUserByIdAsync(int Id);
        Task<User> PostUser(User user);
        Task<User> UpdateUser(int id, User user);
        Task<Boolean> DeleteUser(int id);

    }
}
