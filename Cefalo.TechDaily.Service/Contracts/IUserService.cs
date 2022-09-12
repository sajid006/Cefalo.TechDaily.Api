using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cefalo.TechDaily.Database.Models;
namespace Cefalo.TechDaily.Service.Contracts
{
    public interface IUserService
    {
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int Id);
        Task<User> PostUser(User user);
        Task<User> UpdateUser(int id, User user);
        Task<Boolean> DeleteUser(int id);
    }
}
