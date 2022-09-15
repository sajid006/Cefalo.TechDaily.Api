using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Service.Dto;

namespace Cefalo.TechDaily.Service.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<SignupDto, User>();
            CreateMap<LoginDto, User>();
        }
    }
}
