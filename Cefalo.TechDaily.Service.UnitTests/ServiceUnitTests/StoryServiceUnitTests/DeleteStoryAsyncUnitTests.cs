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

namespace Cefalo.TechDaily.Service.UnitTests.ServiceUnitTests.StoryServiceUnitTests
{
    public class DeleteStoryAsyncUnitTests
    {
        private readonly IStoryService fakeStoryService;
        private readonly IMapper fakeMapper;
        private readonly IStoryRepository fakeStoryRepository;
        private readonly IJwtTokenHandler fakeJwtTokenHandler;
        private readonly BaseDtoValidator<PostStoryDto> fakePostStoryDtoValidator;
        private readonly BaseDtoValidator<UpdateStoryDto> fakeUpdateStoryDtoValidator;
        private readonly FakeStoryData fakeStoryData;
        private readonly Story fakeStory;
        public DeleteStoryAsyncUnitTests()
        {
            fakeStoryRepository = A.Fake<IStoryRepository>();
            fakeMapper = A.Fake<IMapper>();
            fakeJwtTokenHandler = A.Fake<IJwtTokenHandler>();
            fakePostStoryDtoValidator = A.Fake<PostStoryDtoValidator>();
            fakeUpdateStoryDtoValidator = A.Fake<UpdateStoryDtoValidator>();
            fakeStoryService = new StoryService(fakeStoryRepository, fakeMapper, fakeJwtTokenHandler, fakePostStoryDtoValidator, fakeUpdateStoryDtoValidator);
            fakeStoryData = A.Fake<FakeStoryData>();
            fakeStory = fakeStoryData.fakeStory;
        }
        private void ArrangeValidParameters()
        {
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedinUsername()).Returns(fakeStory.AuthorName);
            A.CallTo(() => fakeStoryRepository.GetStoryByIdAsync(fakeStory.Id)).Returns(fakeStory);
            A.CallTo(() => fakeStoryRepository.DeleteStoryAsync(fakeStory.Id)).Returns(true);
        }

        [Fact]
        public async void DeleteStoryAsync_WithValidParameter_GetLoggedInUsernameIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myStory = await fakeStoryService.DeleteStoryAsync(fakeStory.Id);
            //Assert
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedinUsername()).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void DeleteStoryAsync_WithValidParameter_GetStoryByIdAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myStory = await fakeStoryService.DeleteStoryAsync(fakeStory.Id);
            //Assert
            A.CallTo(() => fakeStoryRepository.GetStoryByIdAsync(fakeStory.Id)).MustHaveHappenedOnceExactly();

        }
        [Fact]
        public async void DeleteStoryAsync_WithValidParameter_DeleteStoryAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myStory = await fakeStoryService.DeleteStoryAsync(fakeStory.Id);
            //Assert
            A.CallTo(() => fakeStoryRepository.DeleteStoryAsync(fakeStory.Id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void DeleteStoryAsync_WithValidParameter_ReturnsTrueAfterDeletingStory()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var deleted = await fakeStoryService.DeleteStoryAsync(fakeStory.Id);
            //Assert
            deleted.Should().BeTrue();
        }

        [Fact]
        public async void DeleteStoryAsync_WithLoggedInUserIsNull_ReturnsUnauthorizedException()
        {

            //Arrange
            var errMessage = "You are not authorized to delete this story";
            string? nullUsername = null;
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedinUsername()).Returns(nullUsername);
            //Act
            var exception = await Record.ExceptionAsync(async () => await fakeStoryService.DeleteStoryAsync(fakeStory.Id));
            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be(errMessage);
            exception.GetType().Should().Be(typeof(UnauthorizedException));
        }
        [Fact]
        public async void DeleteStoryAsync_WithCurrentStoryIsNull_ReturnsUnauthorizedException()
        {

            //Arrange
            var errMessage = "You are not authorized to delete this story";
            Story? nullStory = null;
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedinUsername()).Returns(fakeStory.AuthorName);
            A.CallTo(() => fakeStoryRepository.GetStoryByIdAsync(fakeStory.Id)).Returns(nullStory);
            //Act
            var exception = await Record.ExceptionAsync(async () => await fakeStoryService.DeleteStoryAsync(fakeStory.Id));
            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be(errMessage);
            exception.GetType().Should().Be(typeof(UnauthorizedException));
        }
        [Fact]
        public async void DeleteStoryAsync_WithAuthorNameDoesNotMatchLoggedInUsername_ReturnsUnauthorizedException_()
        {

            //Arrange
            var errMessage = "You are not authorized to delete this story";
            A.CallTo(() => fakeStoryRepository.GetStoryByIdAsync(fakeStory.Id)).Returns(fakeStory);
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedinUsername()).Returns("sajid5");
            //Act
            var exception = await Record.ExceptionAsync(async () => await fakeStoryService.DeleteStoryAsync(fakeStory.Id));
            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be(errMessage);
            exception.GetType().Should().Be(typeof(UnauthorizedException)); 
        }
    }
}
