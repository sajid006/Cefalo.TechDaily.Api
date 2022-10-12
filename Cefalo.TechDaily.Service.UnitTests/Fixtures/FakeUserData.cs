using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Service.Dto;
using Cefalo.TechDaily.Service.UnitTests.ServiceUnitTests.Fixtures;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Service.UnitTests.Fixtures
{
    public class FakeUserData
    {
        public User fakeUser;
        public User fakeUser2;
        public UserWithToken fakeUserWithToken;
        public UserDto fakeUserDto;
        public UserDto fakeUserDto2;
        public List<User> fakeUserList;
        public List<UserDto> fakeUserDtoList;
        public SignupDto fakeSignupDto;
        public FakeUserData()
        {
            fakeUser = A.Fake<User>(x => x.WithArgumentsForConstructor(() => new User()));
            fakeUser.Username = "sajid1";
            fakeUser.Name = "Sajid Hasan";
            fakeUser.Email = "sajid1@gmail.com";
            fakeUser.PasswordHash = new byte[10];
            fakeUser.PasswordSalt = new byte[5];
            fakeUser.UpdatedAt = DateTime.Now;
            fakeUser.CreatedAt = DateTime.Now;
            fakeUser.PasswordModifiedAt = DateTime.Now;

            fakeUser2 = A.Fake<User>(x => x.WithArgumentsForConstructor(() => new User()));
            fakeUser2.Username = "sajid2";
            fakeUser2.Name = "Sajid Hasan";
            fakeUser2.Email = "sajid2@gmail.com";
            fakeUser2.PasswordHash = new byte[5];
            fakeUser2.PasswordSalt = new byte[10];
            fakeUser2.UpdatedAt = DateTime.Now;
            fakeUser2.CreatedAt = DateTime.Now;
            fakeUser2.PasswordModifiedAt = DateTime.Now;

            fakeUserList = new List<User>();
            fakeUserList.Add(fakeUser);
            fakeUserList.Add(fakeUser2);

            fakeUserDto = A.Fake<UserDto>(x => x.WithArgumentsForConstructor(() => new UserDto()));
            fakeUserDto.Username = "sajid1";
            fakeUserDto.Name = "Sajid Hasan";
            fakeUserDto.Email = "sajid1@gmail.com";
            fakeUserDto.UpdatedAt = DateTime.Now;
            fakeUserDto.CreatedAt = DateTime.Now;
            fakeUserDto.PasswordModifiedAt = DateTime.Now;

            fakeUserDto2 = A.Fake<UserDto>(x => x.WithArgumentsForConstructor(() => new UserDto()));
            fakeUserDto2.Username = "sajid2";
            fakeUserDto2.Name = "Sajid Hasan";
            fakeUserDto2.Email = "sajid2@gmail.com";
            fakeUserDto2.UpdatedAt = DateTime.Now;
            fakeUserDto2.CreatedAt = DateTime.Now;
            fakeUserDto2.PasswordModifiedAt = DateTime.Now;

            fakeUserDtoList = new List<UserDto>();
            fakeUserDtoList.Add(fakeUserDto);
            fakeUserDtoList.Add(fakeUserDto2);

            fakeUserWithToken = A.Fake<UserWithToken>(x => x.WithArgumentsForConstructor(() => new UserWithToken()));
            fakeUserWithToken.Username = "sajid1";
            fakeUserWithToken.Name = "Sajid Hasan";
            fakeUserWithToken.Email = "sajid1@gmail.com";
            fakeUserWithToken.UpdatedAt = DateTime.Now;
            fakeUserWithToken.CreatedAt = DateTime.Now;
            fakeUserWithToken.PasswordModifiedAt = DateTime.Now;
            fakeUserWithToken.Token = "1234";

            fakeSignupDto = A.Fake<SignupDto>(x => x.WithArgumentsForConstructor(() => new SignupDto()));
            fakeSignupDto.Username = "sajid1";
            fakeSignupDto.Email = "sajid1@gmail.com";
            fakeSignupDto.Name = "Sajid Hasan";
            fakeSignupDto.Password = "1234";
        }
    }
}
