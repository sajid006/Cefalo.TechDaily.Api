using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Service.Contracts
{
    public interface IAuthService
    {
        Task<UserDto?> Login(User user);
        Task<UserDto?> Signup(User user);
        Task<UserDto?> Logout();
    }
}
