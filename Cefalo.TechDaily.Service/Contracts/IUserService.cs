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
        Task<List<UserDto>> GetUsers();
        Task<UserDto> GetUserByUsername(string Username);
        Task<UserWithToken> PostUser(SignupDto request);
        Task<UserDto> UpdateUser(string Username, UpdateUserDto updateUserDto);
        Task<Boolean> DeleteUser(string Username);
    }
}
