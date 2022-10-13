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

namespace Cefalo.TechDaily.Service.UnitTests.ServiceUnitTests.AuthServiceUnitTests
{
    public class GetCurrentUserAsyncUnitTests
    {
        private readonly IUserRepository fakeUserRepository;
        private readonly IMapper fakeMapper;
        private readonly IPasswordHandler fakePasswordHandler;
        private readonly IJwtTokenHandler fakeJwtTokenHandler;
        private readonly BaseDtoValidator<LoginDto> fakeLoginDtoValidator;
        private readonly BaseDtoValidator<SignupDto> fakeSignupDtoValidator;
        private readonly IAuthService fakeAuthService;
        private readonly FakeUserData fakeUserData;
        private readonly SignupDto fakeSignupDto;
        public GetCurrentUserAsyncUnitTests()
        {
            fakeUserRepository = A.Fake<IUserRepository>();
            fakeMapper = A.Fake<IMapper>();
            fakePasswordHandler = A.Fake<IPasswordHandler>();
            fakeJwtTokenHandler = A.Fake<IJwtTokenHandler>();
            fakeLoginDtoValidator = A.Fake<LoginDtoValidator>();
            fakeSignupDtoValidator = A.Fake<SignupDtoValidator>();
            fakeAuthService = new AuthService(fakeUserRepository, fakeMapper, fakePasswordHandler, fakeJwtTokenHandler, fakeLoginDtoValidator, fakeSignupDtoValidator);
            fakeUserData = A.Fake<FakeUserData>();
            fakeSignupDto = fakeUserData.fakeSignupDto;
        }
        private void ArrangeValidParameters()
        {
            A.CallTo(() => fakeJwtTokenHandler.HttpContextExists()).Returns(true);
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedinUsername()).Returns(fakeSignupDto.Username);
        }
        [Fact]
        public async void GetCurrentUserAsync_WithValidParameter_HttpContextExistsIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myUsername = await fakeAuthService.GetCurrentUserAsync();
            //Assert
            A.CallTo(() => fakeJwtTokenHandler.HttpContextExists()).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void GetCurrentUserAsync_WithValidParameter_GetLoggedinUsernameIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myUsername = await fakeAuthService.GetCurrentUserAsync();
            //Assert
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedinUsername()).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void GetCurrentUserAsync_WithValidParameter_ReturnedUsernameIsCorrect()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myUsername = await fakeAuthService.GetCurrentUserAsync();
            //Assert
            myUsername.Should().NotBeNull();
            myUsername.Should().BeEquivalentTo(fakeSignupDto.Username);
        }
        [Fact]
        public async void GetCurrentUserAsync_WithValidParameter_ReturnedNullUsername()
        {
            //Arrange
            ArrangeValidParameters();
            A.CallTo(() => fakeJwtTokenHandler.HttpContextExists()).Returns(false);
            //Act
            var myUsername = await fakeAuthService.GetCurrentUserAsync();
            //Assert
            myUsername.Should().BeNull();
        }

    }
}
