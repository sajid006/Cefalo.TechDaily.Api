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
    public class UpdateStoryAsyncUnitTests
    {
        private readonly IStoryService fakeStoryService;
        private readonly IMapper fakeMapper;
        private readonly IStoryRepository fakeStoryRepository;
        private readonly IJwtTokenHandler fakeJwtTokenHandler;
        private readonly BaseDtoValidator<PostStoryDto> fakePostStoryDtoValidator;
        private readonly BaseDtoValidator<UpdateStoryDto> fakeUpdateStoryDtoValidator;
        private readonly FakeStoryData fakeStoryData;
        private readonly UpdateStoryDto fakeUpdateStoryDto;
        private readonly Story fakeStory;
        public UpdateStoryAsyncUnitTests()
        {
            fakeStoryRepository = A.Fake<IStoryRepository>();
            fakeMapper = A.Fake<IMapper>();
            fakeJwtTokenHandler = A.Fake<IJwtTokenHandler>();
            fakePostStoryDtoValidator = A.Fake<PostStoryDtoValidator>();
            fakeUpdateStoryDtoValidator = A.Fake<UpdateStoryDtoValidator>();
            fakeStoryService = new StoryService(fakeStoryRepository, fakeMapper, fakeJwtTokenHandler, fakePostStoryDtoValidator, fakeUpdateStoryDtoValidator);
            fakeStoryData = A.Fake<FakeStoryData>();
            fakeUpdateStoryDto = fakeStoryData.fakeUpdateStoryDto;
            fakeStory = fakeStoryData.fakeStory;
        }
        private void ArrangeValidParameters()
        {
            A.CallTo(() => fakeUpdateStoryDtoValidator.ValidateDTO(fakeUpdateStoryDto)).DoesNothing();
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedinUsername()).Returns(fakeStory.AuthorName);
            A.CallTo(() => fakeStoryRepository.GetStoryByIdAsync(fakeStory.Id)).Returns(fakeStory);
            A.CallTo(() => fakeMapper.Map<Story>(fakeUpdateStoryDto)).Returns(fakeStory);
            A.CallTo(() => fakeStoryRepository.UpdateStoryAsync(fakeStory.Id, fakeStory)).Returns(fakeStory);
        }
        [Fact]
        public async void UpdateStoryAsync_WithValidParameter_ValidateDTOIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myStory = await fakeStoryService.UpdateStoryAsync(fakeStory.Id, fakeUpdateStoryDto);
            //Assert
            A.CallTo(() => fakeUpdateStoryDtoValidator.ValidateDTO(fakeUpdateStoryDto)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void UpdateStoryAsync_WithValidParameter_GetLoggedInUsernameIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myStory = await fakeStoryService.UpdateStoryAsync(fakeStory.Id, fakeUpdateStoryDto);
            //Assert
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedinUsername()).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void UpdateStoryAsync_WithValidParameter_GetStoryByIdAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myStory = await fakeStoryService.UpdateStoryAsync(fakeStory.Id, fakeUpdateStoryDto);
            //Assert
            A.CallTo(() => fakeStoryRepository.GetStoryByIdAsync(fakeStory.Id)).MustHaveHappenedOnceExactly();
            
        }
        [Fact]
        public async void UpdateStoryAsync_WithValidParameter_UpdateStoryAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myStory = await fakeStoryService.UpdateStoryAsync(fakeStory.Id, fakeUpdateStoryDto);
            //Assert
            A.CallTo(() => fakeStoryRepository.UpdateStoryAsync(fakeStory.Id, fakeStory)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void UpdateStoryAsync_WithValidParameter_MapIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myStory = await fakeStoryService.UpdateStoryAsync(fakeStory.Id, fakeUpdateStoryDto);
            //Assert
            A.CallTo(() => fakeMapper.Map<Story>(fakeUpdateStoryDto)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void UpdateStoryAsync_WithValidParameter_ReturnsUpdatedStoryCorrectly()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myStory = await fakeStoryService.UpdateStoryAsync(fakeStory.Id, fakeUpdateStoryDto);
            //Assert
            myStory.Should().NotBeNull();
            myStory.Should().BeEquivalentTo(fakeStory);
        }

        [Fact]
        public async void UpdateStoryAsync_WithInvalidUpdateStoryDto_ReturnsBadRequestException()
        {

            //Arrange
            var errMessage = "Invalid Title or Description";
            A.CallTo(() => fakeUpdateStoryDtoValidator.ValidateDTO(fakeUpdateStoryDto)).Throws(new BadRequestException(errMessage));
            //Act
            var exception = await Record.ExceptionAsync(async () => await fakeStoryService.UpdateStoryAsync(fakeStory.Id, fakeUpdateStoryDto));
            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be(errMessage);
        }
        [Fact]
        public async void UpdateStoryAsync_WithNullLoggedInUser_ReturnsUnauthorizedException()
        {

            //Arrange
            var errMessage = "You are not authorized to update this story";
            string? nullUsername = null;
            //A.CallTo(() => fakeUpdateStoryDtoValidator.ValidateDTO(fakeUpdateStoryDto)).Throws(new BadRequestException(errMessage));
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedinUsername()).Returns(nullUsername);
            //Act
            var exception = await Record.ExceptionAsync(async () => await fakeStoryService.UpdateStoryAsync(fakeStory.Id, fakeUpdateStoryDto));
            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be(errMessage);
            exception.GetType().Should().Be(typeof(UnauthorizedException));
        }
        [Fact]
        public async void UpdateStoryAsync_WithCurrentStoryIsNull_ReturnsUnauthorizedException_()
        {

            //Arrange
            var errMessage = "You are not authorized to update this story";
            Story? nullStory = null;
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedinUsername()).Returns(fakeStory.AuthorName);
            A.CallTo(() => fakeStoryRepository.GetStoryByIdAsync(fakeStory.Id)).Returns(nullStory);
            //Act
            var exception = await Record.ExceptionAsync(async () => await fakeStoryService.UpdateStoryAsync(fakeStory.Id, fakeUpdateStoryDto));
            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be(errMessage);
            exception.GetType().Should().Be(typeof(UnauthorizedException));
        }
        [Fact]
        public async void UpdateStoryAsync_WithAuthorNameDoesNotMatchLoggedInUsername_ReturnsUnauthorizedException()
        {

            //Arrange
            var errMessage = "You are not authorized to update this story";
            A.CallTo(() => fakeStoryRepository.GetStoryByIdAsync(fakeStory.Id)).Returns(fakeStory);
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedinUsername()).Returns("sajid5");
            //Act
            var exception = await Record.ExceptionAsync(async () => await fakeStoryService.UpdateStoryAsync(fakeStory.Id, fakeUpdateStoryDto));
            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be(errMessage);
            exception.GetType().Should().Be(typeof(UnauthorizedException));
        }

    }
}
