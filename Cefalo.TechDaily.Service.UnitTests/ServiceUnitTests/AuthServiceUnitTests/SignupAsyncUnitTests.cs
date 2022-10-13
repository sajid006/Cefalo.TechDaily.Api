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

namespace Cefalo.TechDaily.Service.UnitTests.ServiceUnitTests.AuthServiceUnitTests
{
    public class SignupAsyncUnitTests
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
        private readonly User fakeUser;
        private readonly UserWithToken fakeUserWithToken;
        public SignupAsyncUnitTests()
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
            fakeUser = fakeUserData.fakeUser;
            fakeUserWithToken = fakeUserData.fakeUserWithToken;
        }
        private void ArrangeValidParameters()
        {
            A.CallTo(() => fakeSignupDtoValidator.ValidateDTO(fakeSignupDto)).DoesNothing();
            A.CallTo(() => fakePasswordHandler.CreatePasswordHash(fakeSignupDto.Password)).Returns(new Tuple<byte[], byte[]>(fakeUser.PasswordSalt, fakeUser.PasswordHash));
            A.CallTo(() => fakeMapper.Map<User>(fakeSignupDto)).Returns(fakeUser);
            A.CallTo(() => fakeUserRepository.PostUserAsync(fakeUser)).Returns(fakeUser);
            A.CallTo(() => fakeMapper.Map<UserWithToken>(fakeUser)).Returns(fakeUserWithToken);
            A.CallTo(() => fakeJwtTokenHandler.CreateToken(fakeUser)).Returns(fakeUserWithToken.Token);
        }
        [Fact]
        public async void SignupAsync_WithValidParameter_ValidateDTOIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myUserWithToken = await fakeAuthService.SignupAsync(fakeSignupDto);
            //Assert
            A.CallTo(() => fakeSignupDtoValidator.ValidateDTO(fakeSignupDto)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void SignupAsync_WithValidParameter_CreatePasswordHashIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myUserWithToken = await fakeAuthService.SignupAsync(fakeSignupDto);
            //Assert
            A.CallTo(() => fakePasswordHandler.CreatePasswordHash(fakeSignupDto.Password)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void SignupAsync_WithValidParameter_MapToUserIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myUserWithToken = await fakeAuthService.SignupAsync(fakeSignupDto);
            //Assert
            A.CallTo(() => fakeMapper.Map<User>(fakeSignupDto)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void SignupAsync_WithValidParameter_SignupAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myUserWithToken = await fakeAuthService.SignupAsync(fakeSignupDto);
            //Assert
            A.CallTo(() => fakeUserRepository.PostUserAsync(fakeUser)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void SignupAsync_WithValidParameter_MapToUserWithTokenIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myUserWithToken = await fakeAuthService.SignupAsync(fakeSignupDto);
            //Assert
            A.CallTo(() => fakeMapper.Map<UserWithToken>(fakeUser)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void SignupAsync_WithValidParameter_CreateTokenIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myUserWithToken = await fakeAuthService.SignupAsync(fakeSignupDto);
            //Assert
            A.CallTo(() => fakeJwtTokenHandler.CreateToken(fakeUser)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void SignupAsync_WithValidParameter_ReturnsCreatedUserCorrectly()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myUserWithToken = await fakeAuthService.SignupAsync(fakeSignupDto);
            //Assert
            myUserWithToken.Should().NotBeNull();
            myUserWithToken.Should().BeEquivalentTo(fakeUserWithToken);
        }
        [Fact]
        public async void SignupAsync_WithInvalidParameter_ReturnedBadRequestException()
        {
            //Arrange
            ArrangeValidParameters();
            var errMessage = "Invalid user input";
            A.CallTo(() => fakeSignupDtoValidator.ValidateDTO(fakeSignupDto)).Throws(new BadRequestException(errMessage));
            //Act
            var ex = await Record.ExceptionAsync(async () => await fakeAuthService.SignupAsync(fakeSignupDto));
            //Assert
            ex.Should().NotBeNull();
            ex.Message.Should().Be(errMessage);
        }
    }
}