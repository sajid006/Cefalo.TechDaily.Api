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

namespace Cefalo.TechDaily.Service.UnitTests.ServiceUnitTests.StoryServiceUnitTests
{
    public class PostStoryAsyncUnitTests
    {
        private readonly IStoryService fakeStoryService;
        private readonly IMapper fakeMapper;
        private readonly IStoryRepository fakeStoryRepository;
        private readonly IJwtTokenHandler fakeJwtTokenHandler;
        private readonly BaseDtoValidator<PostStoryDto> fakePostStoryDtoValidator;
        private readonly BaseDtoValidator<UpdateStoryDto> fakeUpdateStoryDtoValidator;
        private readonly FakeStoryData fakeStoryData;
        private readonly PostStoryDto fakePostStoryDto;
        private readonly Story fakeStory;
        public PostStoryAsyncUnitTests()
        {
            fakeStoryRepository = A.Fake<IStoryRepository>();
            fakeMapper = A.Fake<IMapper>();
            fakeJwtTokenHandler = A.Fake<IJwtTokenHandler>();
            fakePostStoryDtoValidator = A.Fake<PostStoryDtoValidator>();
            fakeUpdateStoryDtoValidator = A.Fake<UpdateStoryDtoValidator>();
            fakeStoryService = new StoryService(fakeStoryRepository, fakeMapper, fakeJwtTokenHandler, fakePostStoryDtoValidator, fakeUpdateStoryDtoValidator);
            fakeStoryData = A.Fake<FakeStoryData>();
            fakePostStoryDto = fakeStoryData.fakePostStoryDto;
            fakeStory = fakeStoryData.fakeStory;
        }
        private void ArrangeValidParameters()
        {
            A.CallTo(() => fakePostStoryDtoValidator.ValidateDTO(fakePostStoryDto)).DoesNothing();
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedinUsername()).Returns(fakePostStoryDto.AuthorName);
            A.CallTo(() => fakeMapper.Map<Story>(fakePostStoryDto)).Returns(fakeStory);
            A.CallTo(() => fakeStoryRepository.PostStoryAsync(fakeStory)).Returns(fakeStory);
        }
        [Fact]
        public async void PostStoryAsync_WithValidParameter_ValidateDTOIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myStory = await fakeStoryService.PostStoryAsync(fakePostStoryDto);
            //Assert
            A.CallTo(() => fakePostStoryDtoValidator.ValidateDTO(fakePostStoryDto)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void PostStoryAsync_WithValidParameter_GetLoggedInUsernameIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myStory = await fakeStoryService.PostStoryAsync(fakePostStoryDto);
            //Assert
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedinUsername()).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void PostStoryAsync_WithValidParameter_PostStoryAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myStory = await fakeStoryService.PostStoryAsync(fakePostStoryDto);
            //Assert
            A.CallTo(() => fakeStoryRepository.PostStoryAsync(fakeStory)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void PostStoryAsync_WithValidParameter_MapIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myStory = await fakeStoryService.PostStoryAsync(fakePostStoryDto);
            //Assert
            A.CallTo(() => fakeMapper.Map<Story>(fakePostStoryDto)).MustHaveHappenedOnceExactly();

        }
        [Fact]
        public async void PostStoryAsync_WithValidParameter_ReturnsPostedStoryCorrectly()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myStory = await fakeStoryService.PostStoryAsync(fakePostStoryDto);
            //Assert
            myStory.Should().NotBeNull();
            myStory.Should().BeEquivalentTo(fakeStory);
        }
        [Fact]
        public async void PostStoryAsync_WithInvalidPostStoryDto_ReturnsBadRequestException()
        {

            //Arrange
            var errMessage = "Invalid Title, Description or AuthorName";
            A.CallTo(() => fakePostStoryDtoValidator.ValidateDTO(fakePostStoryDto)).Throws(new BadRequestException(errMessage));
            //Act
            var exception = await Record.ExceptionAsync(async () => await fakeStoryService.PostStoryAsync(fakePostStoryDto));
            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be(errMessage);
        }
        [Fact]
        public async void PostStoryAsync_WithInvalidAuthorName_ReturnsUnauthorizedException()
        {
            //Arrange
            var errMessage = "You are not logged in";
            //A.CallTo(() => fakePostStoryDtoValidator.ValidateDTO(fakePostStoryDto)).DoesNothing();
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedinUsername()).Returns("WrongUsername");
            //Act
            var exception = await Record.ExceptionAsync(async () => await fakeStoryService.PostStoryAsync(fakePostStoryDto));
            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be(errMessage);
            exception.GetType().Should().Be(typeof(UnauthorizedException));
        }

    }
}
