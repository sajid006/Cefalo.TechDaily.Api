using Cefalo.TechDaily.Api.Controllers;
using Cefalo.TechDaily.Api.Filter;
using Cefalo.TechDaily.Api.Helpers;
using Cefalo.TechDaily.Api.UnitTests.FakeData;
using Cefalo.TechDaily.Api.Wrappers;
using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Service.Contracts;
using Cefalo.TechDaily.Service.CustomExceptions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Api.UnitTests.ControllerUnitTests
{
    public class StoryControllerUnitTests
    {
        private readonly IStoryService fakeStoryService;
        private readonly StoryController fakeStoryController;
        private readonly FakeStoryData fakeStoryData;
        private readonly Story fakeStory;
        private readonly List<Story> fakeStoryList;
        private readonly PaginationFilter fakePaginationFilter;
        public StoryControllerUnitTests()
        {
            fakeStoryService = A.Fake<IStoryService>();
            fakeStoryController = new StoryController(fakeStoryService);
            fakeStoryData = A.Fake<FakeStoryData>();
            fakeStory = fakeStoryData.fakeStory;
            fakeStoryList = fakeStoryData.fakeStoryList;
            fakePaginationFilter = new PaginationFilter(1, 1);
        }
        

        #region GetStoriesAsync
        void ArrangeValidParameters_GetStoriesAsync()
        {
            A.CallTo(() => fakeStoryService.GetStoriesAsync(1,1)).Returns(fakeStoryList);
            A.CallTo(() => fakeStoryService.CountStoriesAsync()).Returns(fakeStoryList.Count);
        }
        [Fact]
        public async void GetStoriesAsync_PaginationFilterIsCalledOnce()
        {

            //Arrange
            ArrangeValidParameters_GetStoriesAsync();
            //Act
            var myStoryList = await fakeStoryController.GetStoriesAsync(fakePaginationFilter);
            //Assert
            A.CallTo(() => fakeStoryService.GetStoriesAsync(1, 1)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void GetStoriesAsync_CountStoriesAsyncIsCalledOnce()
        {

            //Arrange
            ArrangeValidParameters_GetStoriesAsync();
            //Act
            var myStoryList = await fakeStoryController.GetStoriesAsync(fakePaginationFilter);
            //Assert
            A.CallTo(() => fakeStoryService.CountStoriesAsync()).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void GetStoriesAsync_WithValidParameter_ReturnsStoryListCorrectly()
        {
            //Arrange
            ArrangeValidParameters_GetStoriesAsync();
            var fakePagedReponse = PaginationHelper.CreatePagedReponse<Story>(fakeStoryList, fakePaginationFilter, fakeStoryList.Count);
            //Act
            var myStoryList = await fakeStoryController.GetStoriesAsync(fakePaginationFilter);
            //Assert
            myStoryList.Should().NotBeNull();
            myStoryList.Should().BeOfType<ActionResult<IEnumerable<Story>>>();
            myStoryList.Result.Should().BeOfType<OkObjectResult>();
            var myStoryListObject = (OkObjectResult)myStoryList.Result;
            myStoryListObject.Value.Should().BeEquivalentTo(fakePagedReponse);
            myStoryListObject.StatusCode.Should().Be(200);
        }
        #endregion

        #region GetStoryAsync
        void ArrangeValidParameters_GetStoryAsync(Story? returnStory)
        {
            A.CallTo(() => fakeStoryService.GetStoryByIdAsync(1)).Returns(returnStory);
        }
        [Fact]
        public async void GetStoryAsync_WithValidParameter_GetStoryByIdAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters_GetStoryAsync(fakeStory);
            //Act
            var newStory = await fakeStoryController.GetStoryAsync(1);
            //Assert
            A.CallTo(() => fakeStoryService.GetStoryByIdAsync(1)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void GetStoryAsync_WithValidParameter_ReturnsCreatedStoryCorrectly()
        {
            //Arrange
            ArrangeValidParameters_GetStoryAsync(fakeStory);
            //Act
            var newStory = await fakeStoryController.GetStoryAsync(1);
            //Assert
            newStory.Should().NotBeNull();
            newStory.Should().BeOfType<OkObjectResult>();
            //newStory.Should().BeOfType<ActionResult<Response<Story>>>();
            var newStoryObject = (OkObjectResult)newStory;
            newStoryObject.Value.Should().BeEquivalentTo(fakeStory);
            //newStoryObject.Value.Should().BeOfType<ActionResult<Response<Story>>>();
            newStoryObject.StatusCode.Should().Be(200);
        }
        [Fact]
        public async void GetStoryAsync_WithInvalidParameter_ReturnsBadRequest()
        {
            //Arrange
            Story? nullStory = null;
            var errorMessage = "Story not found";
            ArrangeValidParameters_GetStoryAsync(nullStory);
            //Act
            var newStory = await fakeStoryController.GetStoryAsync(1);
            //Assert
            newStory.Should().NotBeNull();
            newStory.Should().BeOfType<BadRequestObjectResult>();
            var newStoryObject = (BadRequestObjectResult)newStory;
            newStoryObject.Value.Should().BeEquivalentTo(errorMessage);
            newStoryObject.StatusCode.Should().Be(400);
        }
        #endregion

        #region GetSearchedStoriesAsync
        void ArrangeValidParameters_GetSearchedStoriesAsync()
        {
            A.CallTo(() => fakeStoryService.GetStoriesAsync(1, 1)).Returns(fakeStoryList);
            A.CallTo(() => fakeStoryService.CountStoriesAsync()).Returns(fakeStoryList.Count);
        }
        [Fact]
        public async void GetStoriesAsync_PaginationFilterIsCalledOnce()
        {

            //Arrange
            ArrangeValidParameters_GetStoriesAsync();
            //Act
            var myStoryList = await fakeStoryController.GetStoriesAsync(fakePaginationFilter);
            //Assert
            A.CallTo(() => fakeStoryService.GetStoriesAsync(1, 1)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void GetStoriesAsync_CountStoriesAsyncIsCalledOnce()
        {

            //Arrange
            ArrangeValidParameters_GetStoriesAsync();
            //Act
            var myStoryList = await fakeStoryController.GetStoriesAsync(fakePaginationFilter);
            //Assert
            A.CallTo(() => fakeStoryService.CountStoriesAsync()).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void GetStoriesAsync_WithValidParameter_ReturnsStoryListCorrectly()
        {
            //Arrange
            ArrangeValidParameters_GetStoriesAsync();
            var fakePagedReponse = PaginationHelper.CreatePagedReponse<Story>(fakeStoryList, fakePaginationFilter, fakeStoryList.Count);
            //Act
            var myStoryList = await fakeStoryController.GetStoriesAsync(fakePaginationFilter);
            //Assert
            myStoryList.Should().NotBeNull();
            myStoryList.Should().BeOfType<ActionResult<IEnumerable<Story>>>();
            myStoryList.Result.Should().BeOfType<OkObjectResult>();
            var myStoryListObject = (OkObjectResult)myStoryList.Result;
            myStoryListObject.Value.Should().BeEquivalentTo(fakePagedReponse);
            myStoryListObject.StatusCode.Should().Be(200);
        }

        #endregion

    }
}
