using System;
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
        Task<List<UserDto>> GetUsersAsync();
        Task<UserDto> GetUserByUsernameAsync(string Username);
        Task<UserWithToken> PostUserAsync(SignupDto request);
        Task<UserDto> UpdateUserAsync(string Username, UpdateUserDto updateUserDto);
        Task<Boolean> DeleteUserAsync(string Username);

    }
}
