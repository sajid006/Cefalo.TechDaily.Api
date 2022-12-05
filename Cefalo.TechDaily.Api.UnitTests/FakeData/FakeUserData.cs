﻿using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Service.Dto;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Api.UnitTests.FakeData
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
        public LoginDto fakeLoginDto;
        public UpdateUserDto fakeUpdateUserDto;
        public UpdateUserDto fakeUpdateUserDto2;
        public UpdateUserDto fakeUpdateUserDto3;
        public FakeUserData()
        {
            fakeUser = A.Fake<User>(x => x.WithArgumentsForConstructor(() => new User()));
            fakeUser.Username = "sajid1";
            fakeUser.Name = "Sajid Hasan";
            fakeUser.Email = "sajid1@gmail.com";
            fakeUser.PasswordHash = new byte[10];
            fakeUser.PasswordSalt = new byte[5];
            fakeUser.UpdatedAt = DateTime.UtcNow;
            fakeUser.CreatedAt = DateTime.UtcNow;
            fakeUser.PasswordModifiedAt = DateTime.UtcNow;

            fakeUser2 = A.Fake<User>(x => x.WithArgumentsForConstructor(() => new User()));
            fakeUser2.Username = "sajid2";
            fakeUser2.Name = "Sajid Hasan";
            fakeUser2.Email = "sajid2@gmail.com";
            fakeUser2.PasswordHash = new byte[5];
            fakeUser2.PasswordSalt = new byte[10];
            fakeUser2.UpdatedAt = DateTime.UtcNow;
            fakeUser2.CreatedAt = DateTime.UtcNow;
            fakeUser2.PasswordModifiedAt = DateTime.UtcNow;

            fakeUserList = new List<User>();
            fakeUserList.Add(fakeUser);
            fakeUserList.Add(fakeUser2);

            fakeUserDto = A.Fake<UserDto>(x => x.WithArgumentsForConstructor(() => new UserDto()));
            fakeUserDto.Username = "sajid1";
            fakeUserDto.Name = "Sajid Hasan";
            fakeUserDto.Email = "sajid1@gmail.com";
            fakeUserDto.UpdatedAt = DateTime.UtcNow;
            fakeUserDto.CreatedAt = DateTime.UtcNow;
            fakeUserDto.PasswordModifiedAt = DateTime.UtcNow;

            fakeUserDto2 = A.Fake<UserDto>(x => x.WithArgumentsForConstructor(() => new UserDto()));
            fakeUserDto2.Username = "sajid2";
            fakeUserDto2.Name = "Sajid Hasan";
            fakeUserDto2.Email = "sajid2@gmail.com";
            fakeUserDto2.UpdatedAt = DateTime.UtcNow;
            fakeUserDto2.CreatedAt = DateTime.UtcNow;
            fakeUserDto2.PasswordModifiedAt = DateTime.UtcNow;

            fakeUserDtoList = new List<UserDto>();
            fakeUserDtoList.Add(fakeUserDto);
            fakeUserDtoList.Add(fakeUserDto2);

            fakeUserWithToken = A.Fake<UserWithToken>(x => x.WithArgumentsForConstructor(() => new UserWithToken()));
            fakeUserWithToken.Username = "sajid1";
            fakeUserWithToken.Name = "Sajid Hasan";
            fakeUserWithToken.Email = "sajid1@gmail.com";
            fakeUserWithToken.UpdatedAt = DateTime.UtcNow;
            fakeUserWithToken.CreatedAt = DateTime.UtcNow;
            fakeUserWithToken.PasswordModifiedAt = DateTime.UtcNow;
            fakeUserWithToken.Token = "ABCD";

            fakeSignupDto = A.Fake<SignupDto>(x => x.WithArgumentsForConstructor(() => new SignupDto()));
            fakeSignupDto.Username = "sajid1";
            fakeSignupDto.Email = "sajid1@gmail.com";
            fakeSignupDto.Name = "Sajid Hasan";
            fakeSignupDto.Password = "12345678";

            fakeLoginDto = A.Fake<LoginDto>(x => x.WithArgumentsForConstructor(() => new LoginDto()));
            fakeLoginDto.Username = "sajid1";
            fakeLoginDto.Password = "12345678";

            fakeUpdateUserDto = A.Fake<UpdateUserDto>(x => x.WithArgumentsForConstructor(() => new UpdateUserDto()));
            fakeUpdateUserDto.Email = "sajid1@gmail.com";
            fakeUpdateUserDto.Name = "Sajid Hasan";
            fakeUpdateUserDto.Password = "12345678";

            fakeUpdateUserDto2 = A.Fake<UpdateUserDto>(x => x.WithArgumentsForConstructor(() => new UpdateUserDto()));
            fakeUpdateUserDto2.Email = "sajid1@gmail.com";
            fakeUpdateUserDto2.Name = "Sajid Hasan";
            fakeUpdateUserDto2.Password = "1234";

            fakeUpdateUserDto3 = A.Fake<UpdateUserDto>(x => x.WithArgumentsForConstructor(() => new UpdateUserDto()));
            fakeUpdateUserDto3.Email = "sajid1@gmail.com";
            fakeUpdateUserDto3.Name = "Sajid Hasan";
            fakeUpdateUserDto3.Password = "";
        }
    }
}

