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
        Task<UserDto?> GetUserById(int Id);
        Task<UserDto?> GetUserByUsername(string Username);
        Task<UserDto> PostUser(SignupDto request);
        Task<UserDto?> UpdateUser(int Id, UpdateUserDto updateUserDto);
        Task<Boolean> DeleteUser(int Id);
    }
}
