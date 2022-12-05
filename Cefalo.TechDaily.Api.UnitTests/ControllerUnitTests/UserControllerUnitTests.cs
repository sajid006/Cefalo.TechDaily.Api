using Cefalo.TechDaily.Api.Controllers;
using Cefalo.TechDaily.Api.Filter;
using Cefalo.TechDaily.Api.Helpers;
using Cefalo.TechDaily.Api.UnitTests.FakeData;
using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Service.Contracts;
using Cefalo.TechDaily.Service.Dto;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Api.UnitTests.ControllerUnitTests
{
    public class UserControllerUnitTests
    {
        private readonly IUserService fakeUserService;
        private readonly UserController fakeUserController;
        private readonly FakeUserData fakeUserData;
        private readonly User fakeUser;
        private readonly List<UserDto> fakeUserDtoList;
        private readonly SignupDto fakeSignupDto;
        private readonly UpdateUserDto fakeUpdateUserDto;
        private readonly UserWithToken fakeUserWithToken;
        private readonly UserDto fakeUserDto;
        public UserControllerUnitTests()
        {
            fakeUserService = A.Fake<IUserService>();
            fakeUserController = new UserController(fakeUserService);
            fakeUserData = A.Fake<FakeUserData>();
            fakeUser = fakeUserData.fakeUser;
            fakeSignupDto = fakeUserData.fakeSignupDto;
            fakeUpdateUserDto = fakeUserData.fakeUpdateUserDto;
            fakeUserWithToken = fakeUserData.fakeUserWithToken;
            fakeUserDto = fakeUserData.fakeUserDto;
            fakeUserDtoList = fakeUserData.fakeUserDtoList;
        }
        #region GetUsersAsync
        void ArrangeValidParameters_GetUsersAsync()
        {
            A.CallTo(() => fakeUserService.GetUsersAsync()).Returns(fakeUserDtoList);
        }
        [Fact]
        public async void GetUsersAsync_GetUsersAsyncIsCalledOnce()
        {

            //Arrange
            ArrangeValidParameters_GetUsersAsync();
            //Act
            var myUserList = await fakeUserController.GetUsersAsync();
            //Assert
            A.CallTo(() => fakeUserService.GetUsersAsync()).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void GetUsersAsync_WithValidParameter_ReturnsUserListCorrectly()
        {
            //Arrange
            ArrangeValidParameters_GetUsersAsync();
            //Act
            var myUserList = await fakeUserController.GetUsersAsync();
            //Assert
            myUserList.Should().NotBeNull();
            myUserList.Should().BeOfType<ActionResult<IEnumerable<UserDto>>>();
            myUserList.Result.Should().BeOfType<OkObjectResult>();
            var myUserListObject = (OkObjectResult)myUserList.Result;
            myUserListObject.Value.Should().BeEquivalentTo(fakeUserDtoList);
            myUserListObject.StatusCode.Should().Be(200);
        }
        #endregion

        #region GetUserByUsernameAsync
        void ArrangeValidParameters_GetUserByUsernameAsync()
        {
            A.CallTo(() => fakeUserService.GetUserByUsernameAsync(fakeUser.Username)).Returns(fakeUserDto);
        }
        [Fact]
        public async void GetUserByUsernameAsync_WithValidParameter_GetUserByUsernameAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters_GetUserByUsernameAsync();
            //Act
            var newUser = await fakeUserController.GetUserByUsernameAsync(fakeUser.Username);
            //Assert
            A.CallTo(() => fakeUserService.GetUserByUsernameAsync(fakeUser.Username)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void GetUserByUsernameAsync_WithValidParameter_ReturnsCreatedUserCorrectly()
        {
            //Arrange
            ArrangeValidParameters_GetUserByUsernameAsync();
            //Act
            var newUser = await fakeUserController.GetUserByUsernameAsync(fakeUser.Username);
            //Assert
            newUser.Should().NotBeNull();
            newUser.Should().BeOfType<OkObjectResult>();
            //newUser.Should().BeOfType<ActionResult<Response<User>>>();
            var newUserObject = (OkObjectResult)newUser;
            newUserObject.Value.Should().BeEquivalentTo(fakeUserDto);
            //newUserObject.Value.Should().BeOfType<ActionResult<Response<User>>>();
            newUserObject.StatusCode.Should().Be(200);
        }
        [Fact]
        public async void GetUserByUsernameAsync_WithInvalidParameter_ReturnsBadRequest()
        {
            //Arrange
            UserDto? nullUser = null;
            var errorMessage = "User not found";
            A.CallTo(() => fakeUserService.GetUserByUsernameAsync(fakeUser.Username)).Returns(nullUser);
            //Act
            var newUser = await fakeUserController.GetUserByUsernameAsync(fakeUser.Username);
            //Assert
            newUser.Should().NotBeNull();
            newUser.Should().BeOfType<BadRequestObjectResult>();
            var newUserObject = (BadRequestObjectResult)newUser;
            newUserObject.Value.Should().BeEquivalentTo(errorMessage);
            newUserObject.StatusCode.Should().Be(400);
        }
        #endregion

        #region PostUserAsync
        void ArrangeValidParameters_PostUserAsync()
        {
            A.CallTo(() => fakeUserService.PostUserAsync(fakeSignupDto)).Returns(fakeUserWithToken);
        }
        [Fact]
        public async void PostUserAsync_WithValidParameter_PostUserAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters_PostUserAsync();
            //Act
            var newUser = await fakeUserController.PostUserAsync(fakeSignupDto);
            //Assert
            A.CallTo(() => fakeUserService.PostUserAsync(fakeSignupDto)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void PostUserAsync_WithValidParameter_ReturnsCreatedUserCorrectly()
        {
            //Arrange
            ArrangeValidParameters_PostUserAsync();
            //Act
            var newUser = await fakeUserController.PostUserAsync(fakeSignupDto);
            //Assert
            newUser.Should().NotBeNull();
            newUser.Should().BeOfType<CreatedResult>();
            //newUser.Should().BeOfType<ActionResult<Response<User>>>();
            var newUserObject = (CreatedResult)newUser;
            newUserObject.Value.Should().BeEquivalentTo(fakeUserWithToken);
            //newUserObject.Value.Should().BeOfType<ActionResult<Response<User>>>();
            newUserObject.StatusCode.Should().Be(201);
        }
        [Fact]
        public async void PostUserAsync_WithInvalidParameter_ReturnsBadRequest()
        {
            //Arrange
            var errorMessage = "Can't create user";
            UserWithToken? nullUser = null;
            A.CallTo(() => fakeUserService.PostUserAsync(fakeSignupDto)).Returns(nullUser);
            //Act
            var newUser = await fakeUserController.PostUserAsync(fakeSignupDto);
            //Assert
            newUser.Should().NotBeNull();
            newUser.Should().BeOfType<BadRequestObjectResult>();
            var newUserObject = (BadRequestObjectResult)newUser;
            newUserObject.Value.Should().BeEquivalentTo(errorMessage);
            newUserObject.StatusCode.Should().Be(400);
        }
        #endregion

        #region UpdateUserAsync
        void ArrangeValidParameters_UpdateUserAsync()
        {
            A.CallTo(() => fakeUserService.UpdateUserAsync(fakeUser.Username, fakeUpdateUserDto)).Returns(fakeUserDto);
        }
        [Fact]
        public async void UpdateUserAsync_WithValidParameter_UpdateUserAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters_UpdateUserAsync();
            //Act
            var newUser = await fakeUserController.UpdateUserAsync(fakeUser.Username, fakeUpdateUserDto);
            //Assert
            A.CallTo(() => fakeUserService.UpdateUserAsync(fakeUser.Username, fakeUpdateUserDto)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void UpdateUserAsync_WithValidParameter_ReturnsUpdatedUserCorrectly()
        {
            //Arrange
            ArrangeValidParameters_UpdateUserAsync();
            //Act
            var newUser = await fakeUserController.UpdateUserAsync(fakeUser.Username, fakeUpdateUserDto);
            //Assert
            newUser.Should().NotBeNull();
            newUser.Should().BeOfType<OkObjectResult>();
            //newUser.Should().BeOfType<ActionResult<Response<User>>>();
            var newUserObject = (OkObjectResult)newUser;
            newUserObject.Value.Should().BeEquivalentTo(fakeUserDto);
            //newUserObject.Value.Should().BeOfType<ActionResult<Response<User>>>();
            newUserObject.StatusCode.Should().Be(200);
        }
        [Fact]
        public async void UpdateUserAsync_WithInvalidParameter_ReturnsBadRequest()
        {
            //Arrange
            var errorMessage = "Can't update user";
            UserDto? nullUser = null;
            A.CallTo(() => fakeUserService.UpdateUserAsync(fakeUser.Username, fakeUpdateUserDto)).Returns(nullUser);
            //Act
            var newUser = await fakeUserController.UpdateUserAsync(fakeUser.Username, fakeUpdateUserDto);
            //Assert
            newUser.Should().NotBeNull();
            newUser.Should().BeOfType<BadRequestObjectResult>();
            var newUserObject = (BadRequestObjectResult)newUser;
            newUserObject.Value.Should().BeEquivalentTo(errorMessage);
            newUserObject.StatusCode.Should().Be(400);
        }
        #endregion

        #region DeleteUserAsync
        void ArrangeValidParameters_DeleteUserAsync()
        {
            A.CallTo(() => fakeUserService.DeleteUserAsync(fakeUser.Username)).Returns(true);
        }
        [Fact]
        public async void DeleteUserAsync_WithValidParameter_DeleteUserAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters_DeleteUserAsync();
            //Act
            var isDeleted = await fakeUserController.DeleteUserAsync(fakeUser.Username);
            //Assert
            A.CallTo(() => fakeUserService.DeleteUserAsync(fakeUser.Username)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void DeleteUserAsync_WithValidParameter_ReturnsTrueIfDeleted()
        {
            //Arrange
            ArrangeValidParameters_DeleteUserAsync();
            //Act
            var isDeleted = await fakeUserController.DeleteUserAsync(fakeUser.Username);
            //Assert
            isDeleted.Should().NotBeNull();
            isDeleted.Should().BeOfType<NoContentResult>();
            //newUser.Should().BeOfType<ActionResult<Response<User>>>();
            var isDeletedObject = (NoContentResult)isDeleted;
            //newUserObject.Value.Should().BeOfType<ActionResult<Response<User>>>();
            isDeletedObject.StatusCode.Should().Be(204);
        }
        [Fact]
        public async void DeleteUserAsync_WithInvalidParameter_ReturnsBadRequest()
        {
            //Arrange
            var errorMessage = "Can't delete user";
            A.CallTo(() => fakeUserService.DeleteUserAsync(fakeUser.Username)).Returns(false);
            //Act
            var newUser = await fakeUserController.DeleteUserAsync(fakeUser.Username);
            //Assert
            newUser.Should().NotBeNull();
            newUser.Should().BeOfType<BadRequestObjectResult>();
            var newUserObject = (BadRequestObjectResult)newUser;
            newUserObject.Value.Should().BeEquivalentTo(errorMessage);
            newUserObject.StatusCode.Should().Be(400);
        }
        #endregion
    }
}
