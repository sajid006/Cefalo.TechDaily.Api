using AutoMapper;
using Cefalo.TechDaily.Service.Contracts;
using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Repository.Contracts;
using FakeItEasy;
using FluentAssertions;
using Cefalo.TechDaily.Service.Services;
using Cefalo.TechDaily.Service.Utils.Contracts;
using Cefalo.TechDaily.Service.DtoValidators;
using Cefalo.TechDaily.Service.Dto;
using Cefalo.TechDaily.Service.UnitTests.Fixtures;
using Xunit.Sdk;

namespace Cefalo.TechDaily.Service.UnitTests.StoryServiceUnitTests
{
    public class GetStoryByIdAsyncUnitTests
    {
        private readonly IStoryService fakeStoryService;
        private readonly IMapper fakeMapper;
        private readonly IStoryRepository fakeStoryRepository;
        private readonly IJwtTokenHandler fakeJwtTokenHandler;
        private readonly BaseDtoValidator<PostStoryDto> fakePostStoryDtoValidator;
        private readonly BaseDtoValidator<UpdateStoryDto> fakeUpdateStoryDtoValidator;
        private readonly TestData testData;
        public GetStoryByIdAsyncUnitTests()
        {
            fakeStoryRepository = A.Fake<IStoryRepository>();
            fakeMapper = A.Fake<IMapper>();
            fakeJwtTokenHandler = A.Fake<IJwtTokenHandler>();
            fakePostStoryDtoValidator = A.Fake<PostStoryDtoValidator>();
            fakeUpdateStoryDtoValidator = A.Fake<UpdateStoryDtoValidator>();
            fakeStoryService = new StoryService(fakeStoryRepository, fakeMapper, fakeJwtTokenHandler, fakePostStoryDtoValidator, fakeUpdateStoryDtoValidator);
            testData = new TestData();
        }
        [Fact]
        public async void GetStoryByIdAsync_WithValidParameter_ReturnsStory()
        {
            //Arrange
            A.CallTo(() => fakeStoryRepository.GetStoriesAsync(testData.fakeStory.Id)).Returns(testData.fakeStory);
            //Act
            var myStory = await fakeStoryService.GetStoryByIdAsync(testData.fakeStory.Id);
            //Assert
            A.CallTo(() => fakeStoryRepository.GetStoriesAsync(testData.fakeStory.Id)).MustHaveHappenedOnceExactly();
            Assert.Equal(testData.fakeStory, myStory);
        }
        [Fact]
        public async void GetStoryByIdAsync_WithInvalidParameter_ReturnsError()
        {

            //Arrange
            Story? nullStory = null;
            A.CallTo(() => fakeStoryRepository.GetStoriesAsync(testData.fakeStory.Id)).Returns(nullStory);
            //Act
            var ex = await Record.ExceptionAsync(async () => await fakeStoryService.GetStoryByIdAsync(testData.fakeStory.Id));
            //Assert
            A.CallTo(() => fakeStoryRepository.GetStoriesAsync(testData.fakeStory.Id)).MustHaveHappenedOnceExactly();
            ex.Should().NotBeNull();
            ex.Message.Should().Be("Story not found");
        }
    }
}
