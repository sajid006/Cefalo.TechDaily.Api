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
        Task<UserDto?> GetUserByIdAsync(int Id);
        Task<UserDto?> GetUserByUsernameAsync(string Username);
        Task<UserDto> PostUser(User user);
        Task<UserDto?> UpdateUser(int Id, UpdateDto updateDto);
        Task<Boolean> DeleteUser(int Id);
    }
}
