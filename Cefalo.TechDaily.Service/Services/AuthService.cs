using AutoMapper;
using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Repository.Contracts;
using Cefalo.TechDaily.Service.Contracts;
using Cefalo.TechDaily.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public AuthService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public Task<UserDto?> Login(User user)
        {
            
        }

        public Task<UserDto?> Logout()
        {
            throw new NotImplementedException();
        }

        public Task<UserDto?> Signup(User user)
        {
            throw new NotImplementedException();
        }
    }
}
