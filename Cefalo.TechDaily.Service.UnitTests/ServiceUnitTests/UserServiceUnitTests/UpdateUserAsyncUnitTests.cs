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

namespace Cefalo.TechDaily.Service.UnitTests.ServiceUnitTests.UserServiceUnitTests
{
    public class UpdateUserAsyncUnitTests
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
        public readonly SignupDto fakeSignupDto;
        public readonly UpdateUserDto fakeUpdateUserDto,fakeUpdateUserDto2,fakeUpdateUserDto3;
        public UpdateUserAsyncUnitTests()
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
            fakeSignupDto = fakeUserData.fakeSignupDto;
            fakeUpdateUserDto = fakeUserData.fakeUpdateUserDto;
            fakeUpdateUserDto2 = fakeUserData.fakeUpdateUserDto2;
            fakeUpdateUserDto3 = fakeUserData.fakeUpdateUserDto3;
        }
        private void ArrangeValidParameters(UpdateUserDto currentFakeUpdateUserDto)
        {
            A.CallTo(() => fakeUpdateUserDtoValidator.ValidateDTO(currentFakeUpdateUserDto)).DoesNothing();
            A.CallTo(() => fakeUserRepository.GetUserByUsernameAsync(fakeUser.Username)).Returns(fakeUser);
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedinUsername()).Returns(fakeUser.Username);
            A.CallTo(() => fakeJwtTokenHandler.GetTokenCreationTime()).Returns(fakeUser.PasswordModifiedAt.AddDays(1).ToString());
            A.CallTo(() => fakeMapper.Map<User>(currentFakeUpdateUserDto)).Returns(fakeUser);
            A.CallTo(() => fakePasswordHandler.CreatePasswordHash(fakeSignupDto.Password)).Returns(new Tuple<byte[], byte[]>(fakeUser.PasswordSalt, fakeUser.PasswordHash));
            A.CallTo(() => fakeUserRepository.UpdateUserAsync(fakeUser.Username, fakeUser)).Returns(fakeUser);
            A.CallTo(() => fakeMapper.Map<UserDto>(fakeUser)).Returns(fakeUserDto);
        }
        [Fact]
        public async void UpdateUserAsync_WithValidParameter_ValidateDTOIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters(fakeUpdateUserDto);
            //Act
            var updatedUser = await fakeUserService.UpdateUserAsync(fakeUser.Username, fakeUpdateUserDto);
            //Assert
            A.CallTo(() => fakeUpdateUserDtoValidator.ValidateDTO(fakeUpdateUserDto)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void UpdateUserAsync_WithValidParameter_GetUserByUsernameAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters(fakeUpdateUserDto);
            //Act
            var updatedUser = await fakeUserService.UpdateUserAsync(fakeUser.Username, fakeUpdateUserDto);
            //Assert
            A.CallTo(() => fakeUserRepository.GetUserByUsernameAsync(fakeUser.Username)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void UpdateUserAsync_WithValidParameter_GetLoggedinUsernameIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters(fakeUpdateUserDto);
            //Act
            var updatedUser = await fakeUserService.UpdateUserAsync(fakeUser.Username, fakeUpdateUserDto);
            //Assert
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedinUsername()).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void UpdateUserAsync_WithValidParameter_GetTokenCreationTimeIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters(fakeUpdateUserDto);
            //Act
            var updatedUser = await fakeUserService.UpdateUserAsync(fakeUser.Username, fakeUpdateUserDto);
            //Assert
            A.CallTo(() => fakeJwtTokenHandler.GetTokenCreationTime()).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void UpdateUserAsync_WithValidParameter_MapToUserIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters(fakeUpdateUserDto);
            //Act
            var updatedUser = await fakeUserService.UpdateUserAsync(fakeUser.Username, fakeUpdateUserDto);
            //Assert
            A.CallTo(() => fakeMapper.Map<User>(fakeUpdateUserDto)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void UpdateUserAsync_WithValidParameter_CreatePasswordHashIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters(fakeUpdateUserDto);
            //Act
            var updatedUser = await fakeUserService.UpdateUserAsync(fakeUser.Username, fakeUpdateUserDto);
            //Assert
            A.CallTo(() => fakePasswordHandler.CreatePasswordHash(fakeSignupDto.Password)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void UpdateUserAsync_WithValidParameter_UpdateUserAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters(fakeUpdateUserDto);
            //Act
            var updatedUser = await fakeUserService.UpdateUserAsync(fakeUser.Username, fakeUpdateUserDto);
            //Assert
            A.CallTo(() => fakeUserRepository.UpdateUserAsync(fakeUser.Username, fakeUser)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void UpdateUserAsync_WithValidParameter_MapToUserDtoIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters(fakeUpdateUserDto);
            //Act
            var updatedUser = await fakeUserService.UpdateUserAsync(fakeUser.Username, fakeUpdateUserDto);
            //Assert
            A.CallTo(() => fakeMapper.Map<UserDto>(fakeUser)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void UpdateUserAsync_WithValidParameter_ReturnsupdatedUserCorrectly()
        {
            //Arrange
            ArrangeValidParameters(fakeUpdateUserDto);
            //Act
            var updatedUser = await fakeUserService.UpdateUserAsync(fakeUser.Username, fakeUpdateUserDto);
            //Assert
            updatedUser.Should().NotBeNull();
            updatedUser.Should().BeEquivalentTo(fakeUserDto);
        }
        [Fact]
        public async void UpdateUserAsync_WithValidParameter_CreatePasswordHashIsNotCalled()
        {
            //Arrange
            ArrangeValidParameters(fakeUpdateUserDto3);
            //Act
            var updatedUser = await fakeUserService.UpdateUserAsync(fakeUser.Username, fakeUpdateUserDto3);
            //Assert
            A.CallTo(() => fakePasswordHandler.CreatePasswordHash(fakeSignupDto.Password)).MustNotHaveHappened();
        }
        [Fact]
        public async void UpdateUserAsync_WithInvalidParameter_ReturnedBadRequestExceptionByValidateDTO()
        {
            //Arrange
            ArrangeValidParameters(fakeUpdateUserDto);
            var errMessage = "Invalid input";
            A.CallTo(() => fakeUpdateUserDtoValidator.ValidateDTO(fakeUpdateUserDto)).Throws(new BadRequestException(errMessage));
            //Act
            var ex = await Record.ExceptionAsync(async () => await fakeUserService.UpdateUserAsync(fakeUser.Username, fakeUpdateUserDto));
            //Assert
            ex.Should().NotBeNull();
            ex.Message.Should().Be(errMessage);
        }
        [Fact]
        public async void UpdateUserAsync_WithInvalidParameter_ReturnedNotFoundExceptionForGetUserByUsernameAsync()
        {
            //Arrange
            ArrangeValidParameters(fakeUpdateUserDto);
            var errMessage = "User not found";
            User? nullUser = null;
            A.CallTo(() => fakeUserRepository.GetUserByUsernameAsync(fakeUser.Username)).Returns(nullUser);
            //Act
            var ex = await Record.ExceptionAsync(async () => await fakeUserService.UpdateUserAsync(fakeUser.Username, fakeUpdateUserDto));
            //Assert
            ex.Should().NotBeNull();
            ex.Message.Should().Be(errMessage);
        }
        [Fact]
        public async void UpdateUserAsync_WithInvalidParameter_ReturnedUnauthorizedExceptionForDifferentUser()
        {
            //Arrange
            ArrangeValidParameters(fakeUpdateUserDto);
            var errMessage = "You are not authorized to update this user's information";
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedinUsername()).Returns("sajid5");
            //Act
            var ex = await Record.ExceptionAsync(async () => await fakeUserService.UpdateUserAsync(fakeUser.Username, fakeUpdateUserDto));
            //Assert
            ex.Should().NotBeNull();
            ex.Message.Should().Be(errMessage);
        }
        [Fact]
        public async void UpdateUserAsync_WithInvalidParameter_ReturnedUnauthorizedExceptionForInvalidTokenCreationTime()
        {
            //Arrange
            ArrangeValidParameters(fakeUpdateUserDto);
            var errMessage = "Please login again";
            string? nullTokenCreationTime = null;
            A.CallTo(() => fakeJwtTokenHandler.GetTokenCreationTime()).Returns(nullTokenCreationTime);
            //Act
            var ex = await Record.ExceptionAsync(async () => await fakeUserService.UpdateUserAsync(fakeUser.Username, fakeUpdateUserDto));
            //Assert
            ex.Should().NotBeNull();
            ex.Message.Should().Be(errMessage);
        }
        [Fact]
        public async void UpdateUserAsync_WithInvalidParameter_ReturnedUnauthorizedExceptionForTokenCreationTimeBeforePasswordModifiedAt()
        {
            //Arrange
            ArrangeValidParameters(fakeUpdateUserDto);
            var errMessage = "Please Login Again";
            A.CallTo(() => fakeJwtTokenHandler.GetTokenCreationTime()).Returns(fakeUser.PasswordModifiedAt.AddDays(-11).ToString());
            //Act
            var ex = await Record.ExceptionAsync(async () => await fakeUserService.UpdateUserAsync(fakeUser.Username, fakeUpdateUserDto));
            //Assert
            ex.Should().NotBeNull();
            ex.Message.Should().Be(errMessage);
        }
        [Fact]
        public async void UpdateUserAsync_WithInvalidParameter_ReturnedBadRequestExceptionForSmallPasswordLength()
        {
            //Arrange
            ArrangeValidParameters(fakeUpdateUserDto2);
            var errMessage = "Password length must be at least 8";
            //Act
            var ex = await Record.ExceptionAsync(async () => await fakeUserService.UpdateUserAsync(fakeUser.Username, fakeUpdateUserDto2));
            //Assert
            ex.Should().NotBeNull();
            ex.Message.Should().Be(errMessage);
        }
    }
}
