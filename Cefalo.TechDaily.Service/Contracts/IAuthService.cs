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
        Task<string> Login(LoginDto request);
        Task<UserDto> Signup(SignupDto request);
        Task<UserDto> Logout();
        
    }
}
