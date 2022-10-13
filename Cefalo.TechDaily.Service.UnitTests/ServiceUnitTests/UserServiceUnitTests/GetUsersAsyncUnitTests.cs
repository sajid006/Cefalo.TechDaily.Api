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
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Service.UnitTests.ServiceUnitTests.UserServiceUnitTests
{
    public class GetUsersAsyncUnitTests
    {
        private readonly IUserService fakeUserService;
        private readonly IMapper fakeMapper;
        private readonly IPasswordHandler fakePasswordHandler;
        private readonly IJwtTokenHandler fakeJwtTokenHandler;
        private readonly IUserRepository fakeUserRepository;
        private readonly BaseDtoValidator<SignupDto> fakeSignupDtoValidator;
        private readonly BaseDtoValidator<UpdateUserDto> fakeUpdateUserDtoValidator;
        private readonly FakeUserData fakeUserData;
        private readonly User fakeUser,fakeUser2;
        private readonly UserDto fakeUserDto, fakeUserDto2;
        private readonly List<User> fakeUserList;
        private readonly List<UserDto> fakeUserDtoList;
        public GetUsersAsyncUnitTests()
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
            fakeUserList = fakeUserData.fakeUserList;
            fakeUserDtoList = fakeUserData.fakeUserDtoList;
        }
        private void ArrangeValidParameters()
        {
            A.CallTo(() => fakeUserRepository.GetUsersAsync()).Returns(fakeUserList);
            A.CallTo(() => fakeMapper.Map<UserDto>(fakeUser)).Returns(fakeUserDto);
            A.CallTo(() => fakeMapper.Map<UserDto>(fakeUser2)).Returns(fakeUserDto2);
        }
        [Fact]
        public async void GetUsersAsync_WithValidParameter_GetUsersAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myUserDtos = await fakeUserService.GetUsersAsync();
            //Assert
            A.CallTo(() => fakeUserRepository.GetUsersAsync()).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void GetUsersAsync_WithValidParameter_MapIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myUserDtos = await fakeUserService.GetUsersAsync();
            //Assert
            A.CallTo(() => fakeMapper.Map<UserDto>(A<object>.That.IsInstanceOf(typeof(User)))).MustHaveHappened(fakeUserList.Count, Times.Exactly);
        }
        [Fact]
        public async void GetUsersAsync_WithValidParameter_ReturnedUserDtoListIsValid()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myUserDtos = await fakeUserService.GetUsersAsync();
            //Assert
            myUserDtos.Should().NotBeNull();
            myUserDtos.Should().BeEquivalentTo(fakeUserDtoList);
            myUserDtos.Should().BeOfType<List<UserDto>>();
        }
    }
}
