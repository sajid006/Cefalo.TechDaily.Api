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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Service.UnitTests.ServiceUnitTests.StoryServiceUnitTests
{
    public class CountStoriesOfAUserAsyncUnitTests
    {
        private readonly IStoryService fakeStoryService;
        private readonly IMapper fakeMapper;
        private readonly IStoryRepository fakeStoryRepository;
        private readonly IJwtTokenHandler fakeJwtTokenHandler;
        private readonly BaseDtoValidator<PostStoryDto> fakePostStoryDtoValidator;
        private readonly BaseDtoValidator<UpdateStoryDto> fakeUpdateStoryDtoValidator;
        private readonly FakeStoryData fakeStoryData;
        private readonly List<Story> fakeStoryList;
        private readonly int fakeStoryCount;
        public CountStoriesOfAUserAsyncUnitTests()
        {
            fakeStoryRepository = A.Fake<IStoryRepository>();
            fakeMapper = A.Fake<IMapper>();
            fakeJwtTokenHandler = A.Fake<IJwtTokenHandler>();
            fakePostStoryDtoValidator = A.Fake<PostStoryDtoValidator>();
            fakeUpdateStoryDtoValidator = A.Fake<UpdateStoryDtoValidator>();
            fakeStoryService = new StoryService(fakeStoryRepository, fakeMapper, fakeJwtTokenHandler, fakePostStoryDtoValidator, fakeUpdateStoryDtoValidator);
            fakeStoryData = A.Fake<FakeStoryData>();
            fakeStoryList = fakeStoryData.fakeStoryList;
            fakeStoryCount = fakeStoryList.Count;
        }
        private void ArrangeValidParameters()
        {
            A.CallTo(() => fakeStoryRepository.CountStoriesOfAUserAsync(fakeStoryList[0].AuthorName)).Returns(fakeStoryCount);
        }

        [Fact]
        public async void CountStoriesOfAUserAsync_WithValidParameter_CountStoriesOfAUserAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myStoryCount = await fakeStoryService.CountStoriesOfAUserAsync(fakeStoryList[0].AuthorName);
            //Assert
            A.CallTo(() => fakeStoryRepository.CountStoriesOfAUserAsync(fakeStoryList[0].AuthorName)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void CountStoriesOfAUserAsync_WithValidParameter_ReturnedCountIsValid()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var myStoryCount = await fakeStoryService.CountStoriesOfAUserAsync(fakeStoryList[0].AuthorName);
            //Assert
            myStoryCount.Should().Be(fakeStoryCount);
        }
    }
}
