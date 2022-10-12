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
    public class DeleteUserAsyncUnitTests
    {
        private readonly IUserService fakeUserService;
        private readonly IMapper fakeMapper;
        private readonly IPasswordHandler fakePasswordHandler;
        private readonly IJwtTokenHandler fakeJwtTokenHandler;
        private readonly IUserRepository fakeUserRepository;
        private readonly BaseDtoValidator<SignupDto> fakeSignupDtoValidator;
        private readonly BaseDtoValidator<UpdateUserDto> fakeUpdateUserDtoValidator;
        private readonly FakeUserData fakeUserData;
        private readonly User fakeUser, fakeUser2;
        private readonly UserDto fakeUserDto, fakeUserDto2;
        private readonly UserWithToken fakeUserWithToken;
        private readonly List<User> fakeUserList;
        private readonly List<UserDto> fakeUserDtoList;
        public DeleteUserAsyncUnitTests()
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
            fakeUser2 = fakeUserData.fakeUser2;
            fakeUserDto = fakeUserData.fakeUserDto;
            fakeUserDto2 = fakeUserData.fakeUserDto2;
            fakeUserWithToken = fakeUserData.fakeUserWithToken;
            fakeUserList = fakeUserData.fakeUserList;
            fakeUserDtoList = fakeUserData.fakeUserDtoList;
        }
        private void ArrangeValidParameters()
        {
            A.CallTo(() => fakeUserRepository.GetUserByUsernameAsync(fakeUser.Username)).Returns(fakeUser);
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedinUsername()).Returns(fakeUser.Username);
            A.CallTo(() => fakeJwtTokenHandler.GetTokenCreationTime()).Returns(fakeUser.PasswordModifiedAt.AddDays(1).ToString());
            A.CallTo(() => fakeUserRepository.DeleteUserAsync(fakeUser.Username)).Returns(true);
        }
        
        [Fact]
        public async void DeleteUserAsync_WithValidParameter_GetUserByUsernameAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var isDeleted = await fakeUserService.DeleteUserAsync(fakeUser.Username);
            //Assert
            A.CallTo(() => fakeUserRepository.GetUserByUsernameAsync(fakeUser.Username)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void DeleteUserAsync_WithValidParameter_GetLoggedinUsernameIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var isDeleted = await fakeUserService.DeleteUserAsync(fakeUser.Username);
            //Assert
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedinUsername()).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void DeleteUserAsync_WithValidParameter_GetTokenCreationTimeIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var isDeleted = await fakeUserService.DeleteUserAsync(fakeUser.Username);
            //Assert
            A.CallTo(() => fakeJwtTokenHandler.GetTokenCreationTime()).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void DeleteUserAsync_WithValidParameter_DeleteUserAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var isDeleted = await fakeUserService.DeleteUserAsync(fakeUser.Username);
            //Assert
            A.CallTo(() => fakeUserRepository.DeleteUserAsync(fakeUser.Username)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void DeleteUserAsync_WithValidParameter_ReturnedUserDtoListIsValid()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var isDeleted = await fakeUserService.DeleteUserAsync(fakeUser.Username);
            //Assert
            isDeleted.Should().BeTrue();
        }
        [Fact]
        public async void DeleteUserAsync_WithInvalidParameter_ReturnedNotFoundException()
        {
            //Arrange
            ArrangeValidParameters();
            User? nullUser = null;
            A.CallTo(() => fakeUserRepository.GetUserByUsernameAsync(fakeUser.Username)).Returns(nullUser);
            //Act
            var ex = await Record.ExceptionAsync(async () => await fakeUserService.DeleteUserAsync(fakeUser.Username));
            //Assert
            ex.Should().NotBeNull();
            ex.Message.Should().Be("User not found");
        }
        [Fact]
        public async void DeleteUserAsync_WithInvalidLoggedinUser_ReturnedUnauthorizedException()
        {
            //Arrange
            ArrangeValidParameters();
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedinUsername()).Returns("sajid2");
            //Act
            var ex = await Record.ExceptionAsync(async () => await fakeUserService.DeleteUserAsync(fakeUser.Username));
            //Assert
            ex.Should().NotBeNull();
            ex.Message.Should().Be("You are not authorized to delete this user");
        }
        [Fact]
        public async void DeleteUserAsync_WithNullTokenCreationTime_ReturnedUnauthorizedException()
        {
            //Arrange
            ArrangeValidParameters();
            String? nullString = null;
            A.CallTo(() => fakeJwtTokenHandler.GetTokenCreationTime()).Returns(nullString);
            //Act
            var ex = await Record.ExceptionAsync(async () => await fakeUserService.DeleteUserAsync(fakeUser.Username));
            //Assert
            ex.Should().NotBeNull();
            ex.Message.Should().Be("Please login again");
        }
        [Fact]
        public async void DeleteUserAsync_WithTokenCreationTimeBeforePasswordModifiedAt_ReturnedUnauthorizedException()
        {
            //Arrange
            ArrangeValidParameters();
            A.CallTo(() => fakeJwtTokenHandler.GetTokenCreationTime()).Returns(fakeUser.PasswordModifiedAt.AddDays(-1).ToString());
            //Act
            var ex = await Record.ExceptionAsync(async () => await fakeUserService.DeleteUserAsync(fakeUser.Username));
            //Assert
            ex.Should().NotBeNull();
            ex.Message.Should().Be("Please Login Again");
        }

    }
}
