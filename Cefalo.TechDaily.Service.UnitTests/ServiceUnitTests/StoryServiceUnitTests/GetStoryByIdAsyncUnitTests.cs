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

namespace Cefalo.TechDaily.Service.UnitTests.ServiceUnitTests.StoryServiceUnitTests
{
    public class GetStoryByIdAsyncUnitTests
    {
        private readonly IStoryService fakeStoryService;
        private readonly IMapper fakeMapper;
        private readonly IStoryRepository fakeStoryRepository;
        private readonly IJwtTokenHandler fakeJwtTokenHandler;
        private readonly BaseDtoValidator<PostStoryDto> fakePostStoryDtoValidator;
        private readonly BaseDtoValidator<UpdateStoryDto> fakeUpdateStoryDtoValidator;
        private readonly FakeStoryData fakeStoryData;
        private readonly Story fakeStory;
        public GetStoryByIdAsyncUnitTests()
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
            A.CallTo(() => fakeStoryRepository.GetStoryByIdAsync(fakeStory.Id)).Returns(fakeStory);
        }
        [Fact]
        public async void GetStoryByIdAsync_WithValidParameter_GetStoryByIdAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myStory = await fakeStoryService.GetStoryByIdAsync(fakeStory.Id);
            //Assert
            A.CallTo(() => fakeStoryRepository.GetStoryByIdAsync(fakeStory.Id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void GetStoryByIdAsync_WithValidParameter_ReturnedStoryIsValid()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myStory = await fakeStoryService.GetStoryByIdAsync(fakeStory.Id);
            //Assert
            myStory.Should().NotBeNull();
            myStory.Should().BeEquivalentTo(fakeStory);
        }

        [Fact]
        public async void GetStoryByIdAsync_WithInvalidParameter_ReturnsError()
        {

            //Arrange
            Story? nullStory = null;
            A.CallTo(() => fakeStoryRepository.GetStoryByIdAsync(fakeStory.Id)).Returns(nullStory);
            //Act
            var ex = await Record.ExceptionAsync(async () => await fakeStoryService.GetStoryByIdAsync(fakeStory.Id));
            //Assert
            ex.Should().NotBeNull();
            ex.Message.Should().Be("Story not found");
        }
    }
}
