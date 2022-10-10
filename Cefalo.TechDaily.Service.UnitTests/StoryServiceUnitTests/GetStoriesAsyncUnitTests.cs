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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Service.UnitTests.StoryServiceUnitTests
{
    public class GetStoriesAsyncUnitTests
    {
        private readonly IStoryService fakeStoryService;
        private readonly IMapper fakeMapper;
        private readonly IStoryRepository fakeStoryRepository;
        private readonly IJwtTokenHandler fakeJwtTokenHandler;
        private readonly BaseDtoValidator<PostStoryDto> fakePostStoryDtoValidator;
        private readonly BaseDtoValidator<UpdateStoryDto> fakeUpdateStoryDtoValidator;
        private readonly TestData testData;
        public GetStoriesAsyncUnitTests()
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
        public async void GetStoryByIdAsync_WithValidParameter_ReturnsStories()
        {
            //Arrange
            A.CallTo(() => fakeStoryRepository.GetStoriesAsync(1,1)).Returns(testData.fakeStoryList);
            //Act
            var myStories = await fakeStoryService.GetStoriesAsync(1,1);
            //Assert
            A.CallTo(() => fakeStoryRepository.GetStoriesAsync(1,1)).MustHaveHappenedOnceExactly();
            Assert.Equal(testData.fakeStoryList, myStories);
        }
        
    }
}
