using AutoMapper;
using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Repository.Contracts;
using Cefalo.TechDaily.Service.Contracts;
using Cefalo.TechDaily.Service.Dto;
using Cefalo.TechDaily.Service.DtoValidators;
using Cefalo.TechDaily.Service.Services;
using Cefalo.TechDaily.Service.UnitTests.Fixtures;
using Cefalo.TechDaily.Service.Utils.Contracts;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Service.UnitTests.ServiceUnitTests.UserServiceUnitTests
{
    public class GetUserByUsernameAsyncUnitTests
    {
        private readonly IUserService fakeUserService;
        private readonly IMapper fakeMapper;
        private readonly IPasswordHandler fakePasswordHandler;
        private readonly IJwtTokenHandler fakeJwtTokenHandler;
        private readonly IUserRepository fakeUserRepository;
        private readonly BaseDtoValidator<SignupDto> fakeSignupDtoValidator;
        private readonly BaseDtoValidator<UpdateUserDto> fakeUpdateUserDtoValidator;
        private readonly FakeUserData fakeUserData;
        private readonly User fakeUser;
        private readonly UserDto fakeUserDto;
        public GetUserByUsernameAsyncUnitTests()
        {

            fakeMapper = A.Fake<IMapper>();
            fakePasswordHandler = A.Fake<IPasswordHandler>();
            fakeJwtTokenHandler = A.Fake<IJwtTokenHandler>();
            fakeUserRepository = A.Fake<IUserRepository>();
            fakeSignupDtoValidator = A.Fake<SignupDtoValidator>();
            fakeUpdateUserDtoValidator = A.Fake<UpdateUserDtoValidator>();
            fakeUserService = new UserService(fakeUserRepository, fakeMapper, fakePasswordHandler, fakeJwtTokenHandler, fakeSignupDtoValidator, fakeUpdateUserDtoValidator);
            fakeUserData = A.Fake<FakeUserData>();
            fakeUser = fakeUserData.fakeUser;
            fakeUserDto = fakeUserData.fakeUserDto;
        }
        private void ArrangeValidParameters()
        {
            A.CallTo(() => fakeUserRepository.GetUserByUsernameAsync(fakeUser.Username)).Returns(fakeUser);
            A.CallTo(() => fakeMapper.Map<UserDto>(fakeUser)).Returns(fakeUserDto);
        }
        [Fact]
        public async void GetUserByUsernameAsync_WithValidParameter_GetUserByUsernameAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myUserDto = await fakeUserService.GetUserByUsernameAsync(fakeUser.Username);
            //Assert
            A.CallTo(() => fakeUserRepository.GetUserByUsernameAsync(fakeUser.Username)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void GetUserByUsernameAsync_WithValidParameter_MapIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myUserDto = await fakeUserService.GetUserByUsernameAsync(fakeUser.Username);
            //Assert
            A.CallTo(() => fakeMapper.Map<UserDto>(fakeUser)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void GetUserByUsernameAsync_WithValidParameter_ReturnedUserDtoIsValid()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myUserDto = await fakeUserService.GetUserByUsernameAsync(fakeUser.Username);
            //Assert
            myUserDto.Should().NotBeNull();
            myUserDto.Should().BeEquivalentTo(fakeUserDto);
        }
        [Fact]
        public async void GetUserByUsernameAsync_WithInvalidParameter_ReturnedNotFoundException()
        {
            //Arrange
            User? nullUser = null;
            A.CallTo(() => fakeUserRepository.GetUserByUsernameAsync(fakeUser.Username)).Returns(nullUser);
            //Act
            var ex = await Record.ExceptionAsync(async () => await fakeUserService.GetUserByUsernameAsync(fakeUser.Username));
            //Assert
            ex.Should().NotBeNull();
            ex.Message.Should().Be("User not found");
        }
    }
}

