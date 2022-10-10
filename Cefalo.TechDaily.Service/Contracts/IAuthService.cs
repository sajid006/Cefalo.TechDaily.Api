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
        Task<UserWithToken> LoginAsync(LoginDto request);
        Task<UserWithToken> SignupAsync(SignupDto request);
        Task<string?> GetCurrentUserAsync();
        void LogoutAsync();
        
    }
}
