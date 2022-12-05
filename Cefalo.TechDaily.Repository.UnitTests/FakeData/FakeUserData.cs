using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Service.Dto;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Repository.UnitTests.FakeData
{
    public class FakeUserData
    {
        public User fakeUser,fakeUser2,fakeUser3, fakeUser4;
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
            fakeUser.UpdatedAt = TrimMilliseconds(DateTime.UtcNow);
            fakeUser.CreatedAt = TrimMilliseconds(DateTime.UtcNow);
            fakeUser.PasswordModifiedAt = TrimMilliseconds(DateTime.UtcNow);

            fakeUser2 = A.Fake<User>(x => x.WithArgumentsForConstructor(() => new User()));
            fakeUser2.Username = "sajid2";
            fakeUser2.Name = "Sajid Hasan";
            fakeUser2.Email = "sajid2@gmail.com";
            fakeUser2.PasswordHash = new byte[5];
            fakeUser2.PasswordSalt = new byte[10];
            fakeUser2.UpdatedAt = TrimMilliseconds(DateTime.UtcNow);
            fakeUser2.CreatedAt = TrimMilliseconds(DateTime.UtcNow);
            fakeUser2.PasswordModifiedAt = TrimMilliseconds(DateTime.UtcNow);

            fakeUser3 = A.Fake<User>(x => x.WithArgumentsForConstructor(() => new User()));
            fakeUser3.Username = "sajid3";
            fakeUser3.Name = "Sajid Hasan";
            fakeUser3.Email = "sajid3@gmail.com";
            fakeUser3.PasswordHash = new byte[5];
            fakeUser3.PasswordSalt = new byte[10];
            fakeUser3.UpdatedAt = TrimMilliseconds(DateTime.UtcNow);
            fakeUser3.CreatedAt = TrimMilliseconds(DateTime.UtcNow);
            fakeUser3.PasswordModifiedAt = TrimMilliseconds(DateTime.UtcNow);

            fakeUser4 = A.Fake<User>(x => x.WithArgumentsForConstructor(() => new User()));
            fakeUser4.Username = "sajid4";
            fakeUser4.Name = "Sajid Hasan";
            fakeUser4.Email = "sajid4@gmail.com";
            fakeUser4.PasswordHash = new byte[5];
            fakeUser4.PasswordSalt = new byte[10];
            fakeUser4.UpdatedAt = TrimMilliseconds(DateTime.UtcNow);
            fakeUser4.CreatedAt = TrimMilliseconds(DateTime.UtcNow);
            fakeUser4.PasswordModifiedAt = TrimMilliseconds(DateTime.UtcNow);

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
        public static DateTime TrimMilliseconds(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0, 0, dt.Kind);
        }
    }
}
