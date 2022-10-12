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
using Xunit.Sdk;

namespace Cefalo.TechDaily.Service.UnitTests.ServiceUnitTests.UserServiceUnitTests
{
    public class PostUserAsyncUnitTests
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
        public readonly SignupDto fakeSignupDto;
        public PostUserAsyncUnitTests()
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
            fakeSignupDto = fakeUserData.fakeSignupDto;
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
        public async void PostUserAsync_WithValidParameter_ValidateDTOOfSignupDtoValidatorIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var createdUser = await fakeUserService.PostUserAsync(fakeSignupDto);
            //Assert
            A.CallTo(() => fakeSignupDtoValidator.ValidateDTO(fakeSignupDto)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void PostUserAsync_WithValidParameter_CreatePasswordHashIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var createdUser = await fakeUserService.PostUserAsync(fakeSignupDto);
            //Assert
            A.CallTo(() => fakePasswordHandler.CreatePasswordHash(fakeSignupDto.Password)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void PostUserAsync_WithValidParameter_MapToUserIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var createdUser = await fakeUserService.PostUserAsync(fakeSignupDto);
            //Assert
            A.CallTo(() => fakeMapper.Map<User>(fakeSignupDto)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void PostUserAsync_WithValidParameter_PostUserAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var createdUser = await fakeUserService.PostUserAsync(fakeSignupDto);
            //Assert
            A.CallTo(() => fakeUserRepository.PostUserAsync(fakeUser)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void PostUserAsync_WithValidParameter_MapToUserWithTokenIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var createdUser = await fakeUserService.PostUserAsync(fakeSignupDto);
            //Assert
            A.CallTo(() => fakeMapper.Map<UserWithToken>(fakeUser)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void PostUserAsync_WithValidParameter_CreateTokenIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var createdUser = await fakeUserService.PostUserAsync(fakeSignupDto);
            //Assert
            A.CallTo(() => fakeJwtTokenHandler.CreateToken(fakeUser)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void PostUserAsync_WithValidParameter_ValidateDTOOfUserWithTokenValidatorIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var createdUser = await fakeUserService.PostUserAsync(fakeSignupDto);
            //Assert
            A.CallTo(() => fakeUserWithTokenValidator.ValidateDTO(fakeUserWithToken)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void PostUserAsync_WithValidParameter_ReturnsCreatedUserCorrectly()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var createdUser = await fakeUserService.PostUserAsync(fakeSignupDto);
            //Assert
            createdUser.Should().NotBeNull();
            createdUser.Should().BeEquivalentTo(fakeUserWithToken);
        }
        [Fact]
        public async void PostUserAsync_WithInvalidParameter_ReturnedBadRequestException()
        {
            //Arrange
            ArrangeValidParameters();
            var errMessage = "Invalid input";
            A.CallTo(() => fakeSignupDtoValidator.ValidateDTO(fakeSignupDto)).Throws(new BadRequestException(errMessage));
            //Act
            var ex = await Record.ExceptionAsync(async () => await fakeUserService.PostUserAsync(fakeSignupDto));
            //Assert
            ex.Should().NotBeNull();
            ex.Message.Should().Be(errMessage);
        }
        
    }
}
