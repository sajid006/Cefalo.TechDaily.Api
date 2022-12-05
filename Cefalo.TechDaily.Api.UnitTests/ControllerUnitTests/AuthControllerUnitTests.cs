using Cefalo.TechDaily.Api.Controllers;
using Cefalo.TechDaily.Api.UnitTests.FakeData;
using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Service.Contracts;
using Cefalo.TechDaily.Service.Dto;
using Cefalo.TechDaily.Service.Utils.Contracts;
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
    public class AuthControllerUnitTests
    {
        private readonly IAuthService fakeAuthService;
        private readonly IJwtTokenHandler fakeJwtTokenHandler;
        private readonly AuthController fakeAuthController;
        private readonly FakeUserData fakeUserData;
        private readonly SignupDto fakeSignupDto;
        private readonly UserWithToken fakeUserWithToken;
        private readonly LoginDto fakeLoginDto;
        private readonly User fakeUser;
        public AuthControllerUnitTests()
        {
            fakeAuthService = A.Fake<IAuthService>();
            fakeJwtTokenHandler = A.Fake<IJwtTokenHandler>();
            fakeAuthController = new AuthController(fakeAuthService, fakeJwtTokenHandler);
            fakeUserData = A.Fake<FakeUserData>();
            fakeUser = fakeUserData.fakeUser;
            fakeSignupDto = fakeUserData.fakeSignupDto;
            fakeUserWithToken = fakeUserData.fakeUserWithToken;
            fakeLoginDto = fakeUserData.fakeLoginDto;
        }
        #region SignupAsync
        void ArrangeValidParameters_SignupAsync()
        {
            A.CallTo(() => fakeAuthService.SignupAsync(fakeSignupDto)).Returns(fakeUserWithToken);
        }
        [Fact]
        public async void SignupAsync_WithValidParameter_SignupAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters_SignupAsync();
            //Act
            var newUser = await fakeAuthController.SignupAsync(fakeSignupDto);
            //Assert
            A.CallTo(() => fakeAuthService.SignupAsync(fakeSignupDto)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void SignupAsync_WithValidParameter_ReturnsCreatedUserCorrectly()
        {
            //Arrange
            ArrangeValidParameters_SignupAsync();
            //Act
            var newUser = await fakeAuthController.SignupAsync(fakeSignupDto);
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
        public async void SignupAsync_WithInvalidParameter_ReturnsBadRequest()
        {
            //Arrange
            var errorMessage = "Can't create user";
            UserWithToken? nullUser = null;
            A.CallTo(() => fakeAuthService.SignupAsync(fakeSignupDto)).Returns(nullUser);
            //Act
            var newUser = await fakeAuthController.SignupAsync(fakeSignupDto);
            //Assert
            newUser.Should().NotBeNull();
            newUser.Should().BeOfType<BadRequestObjectResult>();
            var newUserObject = (BadRequestObjectResult)newUser;
            newUserObject.Value.Should().BeEquivalentTo(errorMessage);
            newUserObject.StatusCode.Should().Be(400);
        }
        #endregion

        #region LoginAsync
        void ArrangeValidParameters_LoginAsync()
        {
            A.CallTo(() => fakeAuthService.LoginAsync(fakeLoginDto)).Returns(fakeUserWithToken);
        }
        [Fact]
        public async void LoginAsync_WithValidParameter_LoginAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters_LoginAsync();
            //Act
            var newUser = await fakeAuthController.LoginAsync(fakeLoginDto);
            //Assert
            A.CallTo(() => fakeAuthService.LoginAsync(fakeLoginDto)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void LoginAsync_WithValidParameter_ReturnsLoggedinUserCorrectly()
        {
            //Arrange
            ArrangeValidParameters_LoginAsync();
            //Act
            var newUser = await fakeAuthController.LoginAsync(fakeLoginDto);
            //Assert
            newUser.Should().NotBeNull();
            newUser.Should().BeOfType<OkObjectResult>();
            var newUserObject = (OkObjectResult)newUser;
            newUserObject.Value.Should().BeEquivalentTo(fakeUserWithToken);
            newUserObject.StatusCode.Should().Be(200);
        }
        [Fact]
        public async void LoginAsync_WithInvalidParameter_ReturnsBadRequest()
        {
            //Arrange
            var errorMessage = "Failed To Login";
            UserWithToken? nullUser = null;
            A.CallTo(() => fakeAuthService.LoginAsync(fakeLoginDto)).Returns(nullUser);
            //Act
            var newUser = await fakeAuthController.LoginAsync(fakeLoginDto);
            //Assert
            newUser.Should().NotBeNull();
            newUser.Should().BeOfType<BadRequestObjectResult>();
            var newUserObject = (BadRequestObjectResult)newUser;
            newUserObject.Value.Should().BeEquivalentTo(errorMessage);
            newUserObject.StatusCode.Should().Be(400);
        }
        #endregion

        #region VerifyAsync
        void ArrangeValidParameters_VerifyAsync()
        {
            A.CallTo(() => fakeAuthService.GetCurrentUserAsync()).Returns(fakeUser.Username);
        }
        [Fact]
        public async void VerifyAsync_WithValidParameter_VerifyAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters_VerifyAsync();
            //Act
            var currentUser = await fakeAuthController.VerifyAsync();
            //Assert
            A.CallTo(() => fakeAuthService.GetCurrentUserAsync()).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void VerifyAsync_WithValidParameter_ReturnsCreatedUserCorrectly()
        {
            //Arrange
            ArrangeValidParameters_VerifyAsync();
            //Act
            var currentUser = await fakeAuthController.VerifyAsync();
            //Assert
            currentUser.Should().NotBeNull();
            currentUser.Should().BeOfType<OkObjectResult>();
            //newUser.Should().BeOfType<ActionResult<Response<User>>>();
            var currentUserObject = (OkObjectResult)currentUser;
            currentUserObject.Value.Should().BeEquivalentTo(fakeUser.Username);
            //newUserObject.Value.Should().BeOfType<ActionResult<Response<User>>>();
            currentUserObject.StatusCode.Should().Be(200);
        }
        [Fact]
        public async void VerifyAsync_WithInvalidParameter_ReturnsBadRequest()
        {
            //Arrange
            String? nullUserUsername = null;
            A.CallTo(() => fakeAuthService.GetCurrentUserAsync()).Returns(nullUserUsername);
            //Act
            var currentUser = await fakeAuthController.VerifyAsync();
            //Assert
            currentUser.Should().NotBeNull();
            currentUser.Should().BeOfType<UnauthorizedResult>();
            var currentUserObject = (UnauthorizedResult)currentUser;
            //newUserObject.Value.Should().BeEquivalentTo(null);
            currentUserObject.StatusCode.Should().Be(401);
        }
        #endregion
    }
}
