using AutoMapper;
using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Repository.Contracts;
using Cefalo.TechDaily.Service.Contracts;
using Cefalo.TechDaily.Service.CustomExceptions;
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
    public class LoginAsyncUnitTests
    {
        private readonly IUserRepository fakeUserRepository;
        private readonly IMapper fakeMapper;
        private readonly IPasswordHandler fakePasswordHandler;
        private readonly IJwtTokenHandler fakeJwtTokenHandler;
        private readonly BaseDtoValidator<LoginDto> fakeLoginDtoValidator;
        private readonly BaseDtoValidator<SignupDto> fakeSignupDtoValidator;
        private readonly IAuthService fakeAuthService;
        private readonly FakeUserData fakeUserData;
        private readonly LoginDto fakeLoginDto;
        private readonly User fakeUser;
        private readonly UserWithToken fakeUserWithToken;
        public LoginAsyncUnitTests()
        {
            fakeUserRepository = A.Fake<IUserRepository>();
            fakeMapper = A.Fake<IMapper>();
            fakePasswordHandler = A.Fake<IPasswordHandler>();
            fakeJwtTokenHandler = A.Fake<IJwtTokenHandler>();
            fakeLoginDtoValidator = A.Fake<LoginDtoValidator>();
            fakeSignupDtoValidator = A.Fake<SignupDtoValidator>();
            fakeAuthService = new AuthService(fakeUserRepository, fakeMapper, fakePasswordHandler, fakeJwtTokenHandler, fakeLoginDtoValidator, fakeSignupDtoValidator);
            fakeUserData = A.Fake<FakeUserData>();
            fakeLoginDto = fakeUserData.fakeLoginDto;
            fakeUser = fakeUserData.fakeUser;
            fakeUserWithToken = fakeUserData.fakeUserWithToken;
        }
        private void ArrangeValidParameters()
        {
            A.CallTo(() => fakeLoginDtoValidator.ValidateDTO(fakeLoginDto)).DoesNothing();
            A.CallTo(() => fakeUserRepository.GetUserByUsernameAsync(fakeLoginDto.Username)).Returns(fakeUser);
            A.CallTo(() => fakePasswordHandler.VerifyPasswordHash(fakeLoginDto.Password, A<byte[]>.That.IsInstanceOf(typeof(byte[])), A<byte[]>.That.IsInstanceOf(typeof(byte[])))).Returns(true);
            A.CallTo(() => fakeMapper.Map<UserWithToken>(fakeUser)).Returns(fakeUserWithToken);
            A.CallTo(() => fakeJwtTokenHandler.CreateToken(fakeUser)).Returns(fakeUserWithToken.Token);
        }
        [Fact]
        public async void LoginAsync_WithValidParameter_ValidateDTOIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myUsername = await fakeAuthService.LoginAsync(fakeLoginDto);
            //Assert
            A.CallTo(() => fakeLoginDtoValidator.ValidateDTO(fakeLoginDto)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void LoginAsync_WithValidParameter_GetUserByUsernameAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myUsername = await fakeAuthService.LoginAsync(fakeLoginDto);
            //Assert
            A.CallTo(() => fakeUserRepository.GetUserByUsernameAsync(fakeLoginDto.Username)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void LoginAsync_WithValidParameter_VerifyPassWordHashIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myUsername = await fakeAuthService.LoginAsync(fakeLoginDto);
            //Assert
            A.CallTo(() => fakePasswordHandler.VerifyPasswordHash(fakeLoginDto.Password, A<byte[]>.That.IsInstanceOf(typeof(byte[])), A<byte[]>.That.IsInstanceOf(typeof(byte[])))).MustHaveHappenedOnceExactly();

        }
        [Fact]
        public async void LoginAsync_WithValidParameter_MapIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myUsername = await fakeAuthService.LoginAsync(fakeLoginDto);
            //Assert
            A.CallTo(() => fakeMapper.Map<UserWithToken>(fakeUser)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void LoginAsync_WithValidParameter_CreateTokenIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myUsername = await fakeAuthService.LoginAsync(fakeLoginDto);
            //Assert
            A.CallTo(() => fakeJwtTokenHandler.CreateToken(fakeUser)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void LoginAsync_WithInvalidParameter_ReturnsBadRequestExceptionFfromDtoValidator()
        {
            //Arrange
            ArrangeValidParameters();
            var errMessage = "Invalid input";
            A.CallTo(() => fakeLoginDtoValidator.ValidateDTO(fakeLoginDto)).Throws(new BadRequestException(errMessage));
            //Act
            var ex = await Record.ExceptionAsync(async () => await fakeAuthService.LoginAsync(fakeLoginDto));

            //Assert
            ex.Should().NotBeNull();
            ex.Message.Should().Be(errMessage);
            ex.GetType().Should().Be(typeof(BadRequestException));
        }
        [Fact]
        public async void LoginAsync_WithInvalidParameter_ReturnsBadRequestExceptionForWrongUsername()
        {
            //Arrange
            ArrangeValidParameters();
            User? nullUser = null;
            var errMessage = "Invalid username or password";
            A.CallTo(() => fakeUserRepository.GetUserByUsernameAsync(fakeLoginDto.Username)).Returns(nullUser);
            //Act
            var ex = await Record.ExceptionAsync(async () => await fakeAuthService.LoginAsync(fakeLoginDto));

            //Assert
            ex.Should().NotBeNull();
            ex.Message.Should().Be(errMessage);
            ex.GetType().Should().Be(typeof(BadRequestException));
        }
        [Fact]
        public async void LoginAsync_WithInvalidParameter_ReturnsBadRequestExceptionForWrongPassword()
        {
            //Arrange
            ArrangeValidParameters();
            var errMessage = "Invalid username or password";
            A.CallTo(() => fakePasswordHandler.VerifyPasswordHash(fakeLoginDto.Password, A<byte[]>.That.IsInstanceOf(typeof(byte[])), A<byte[]>.That.IsInstanceOf(typeof(byte[])))).Returns(false);
            //Act
            var ex = await Record.ExceptionAsync(async () => await fakeAuthService.LoginAsync(fakeLoginDto));

            //Assert
            ex.Should().NotBeNull();
            ex.Message.Should().Be(errMessage);
            ex.GetType().Should().Be(typeof(BadRequestException));
        }
    }
}
