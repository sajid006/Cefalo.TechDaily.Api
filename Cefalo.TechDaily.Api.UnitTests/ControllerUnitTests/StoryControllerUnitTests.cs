using Cefalo.TechDaily.Api.Controllers;
using Cefalo.TechDaily.Api.Filter;
using Cefalo.TechDaily.Api.Helpers;
using Cefalo.TechDaily.Api.UnitTests.FakeData;
using Cefalo.TechDaily.Api.Wrappers;
using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Service.Contracts;
using Cefalo.TechDaily.Service.CustomExceptions;
using Cefalo.TechDaily.Service.Dto;
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
        private readonly PostStoryDto fakePostStoryDto;
        private readonly UpdateStoryDto fakeUpdateStoryDto;
        public StoryControllerUnitTests()
        {
            fakeStoryService = A.Fake<IStoryService>();
            fakeStoryController = new StoryController(fakeStoryService);
            fakeStoryData = A.Fake<FakeStoryData>();
            fakeStory = fakeStoryData.fakeStory;
            fakeStoryList = fakeStoryData.fakeStoryList;
            fakePaginationFilter = new PaginationFilter(1, 1);
            fakePostStoryDto = fakeStoryData.fakePostStoryDto;
            fakeUpdateStoryDto = fakeStoryData.fakeUpdateStoryDto;
        }
        

        #region GetStoriesAsync
        void ArrangeValidParameters_GetStoriesAsync()
        {
            A.CallTo(() => fakeStoryService.GetStoriesAsync(1,1)).Returns(fakeStoryList);
            A.CallTo(() => fakeStoryService.CountStoriesAsync()).Returns(fakeStoryList.Count);
        }
        [Fact]
        public async void GetStoriesAsync_GetStoriesAsyncIsCalledOnce()
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

        #region PostStoryAsync
        void ArrangeValidParameters_PostStoryAsync()
        {
            A.CallTo(() => fakeStoryService.PostStoryAsync(fakePostStoryDto)).Returns(fakeStory);
        }
        [Fact]
        public async void PostStoryAsync_WithValidParameter_PostStoryAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters_PostStoryAsync();
            //Act
            var newStory = await fakeStoryController.PostStoryAsync(fakePostStoryDto);
            //Assert
            A.CallTo(() => fakeStoryService.PostStoryAsync(fakePostStoryDto)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void PostStoryAsync_WithValidParameter_ReturnsCreatedStoryCorrectly()
        {
            //Arrange
            ArrangeValidParameters_PostStoryAsync();
            //Act
            var newStory = await fakeStoryController.PostStoryAsync(fakePostStoryDto);
            //Assert
            newStory.Should().NotBeNull();
            newStory.Should().BeOfType<CreatedResult>();
            //newStory.Should().BeOfType<ActionResult<Response<Story>>>();
            var newStoryObject = (CreatedResult)newStory;
            newStoryObject.Value.Should().BeEquivalentTo(fakeStory);
            //newStoryObject.Value.Should().BeOfType<ActionResult<Response<Story>>>();
            newStoryObject.StatusCode.Should().Be(201);
        }
        [Fact]
        public async void PostStoryAsync_WithInvalidParameter_ReturnsBadRequest()
        {
            //Arrange
            var errorMessage = "Cant post story";
            Story? nullStory = null;
            A.CallTo(() => fakeStoryService.PostStoryAsync(fakePostStoryDto)).Returns(nullStory);
            //Act
            var newStory = await fakeStoryController.PostStoryAsync(fakePostStoryDto);
            //Assert
            newStory.Should().NotBeNull();
            newStory.Should().BeOfType<BadRequestObjectResult>();
            var newStoryObject = (BadRequestObjectResult)newStory;
            newStoryObject.Value.Should().BeEquivalentTo(errorMessage);
            newStoryObject.StatusCode.Should().Be(400);
        }
        #endregion

        #region UpdateStoryAsync
        void ArrangeValidParameters_UpdateStoryAsync()
        {
            A.CallTo(() => fakeStoryService.UpdateStoryAsync(fakeStory.Id, fakeUpdateStoryDto)).Returns(fakeStory);
        }
        [Fact]
        public async void UpdateStoryAsync_WithValidParameter_UpdateStoryAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters_UpdateStoryAsync();
            //Act
            var newStory = await fakeStoryController.UpdateStoryAsync(fakeStory.Id, fakeUpdateStoryDto);
            //Assert
            A.CallTo(() => fakeStoryService.UpdateStoryAsync(fakeStory.Id, fakeUpdateStoryDto)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void UpdateStoryAsync_WithValidParameter_ReturnsUpdatedStoryCorrectly()
        {
            //Arrange
            ArrangeValidParameters_UpdateStoryAsync();
            //Act
            var newStory = await fakeStoryController.UpdateStoryAsync(fakeStory.Id, fakeUpdateStoryDto);
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
        public async void UpdateStoryAsync_WithInvalidParameter_ReturnsBadRequest()
        {
            //Arrange
            var errorMessage = "Can't update story";
            Story? nullStory = null;
            A.CallTo(() => fakeStoryService.UpdateStoryAsync(fakeStory.Id, fakeUpdateStoryDto)).Returns(nullStory);
            //Act
            var newStory = await fakeStoryController.UpdateStoryAsync(fakeStory.Id, fakeUpdateStoryDto);
            //Assert
            newStory.Should().NotBeNull();
            newStory.Should().BeOfType<BadRequestObjectResult>();
            var newStoryObject = (BadRequestObjectResult)newStory;
            newStoryObject.Value.Should().BeEquivalentTo(errorMessage);
            newStoryObject.StatusCode.Should().Be(400);
        }
        #endregion

        #region DeleteStoryAsync
        void ArrangeValidParameters_DeleteStoryAsync()
        {
            A.CallTo(() => fakeStoryService.DeleteStoryAsync(fakeStory.Id)).Returns(true);
        }
        [Fact]
        public async void DeleteStoryAsync_WithValidParameter_DeleteStoryAsyncIsCalledOnce()
        {
            //Arrange
            ArrangeValidParameters_DeleteStoryAsync();
            //Act
            var isDeleted = await fakeStoryController.DeleteStoryAsync(fakeStory.Id);
            //Assert
            A.CallTo(() => fakeStoryService.DeleteStoryAsync(fakeStory.Id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void DeleteStoryAsync_WithValidParameter_ReturnsTrueIfDeleted()
        {
            //Arrange
            ArrangeValidParameters_DeleteStoryAsync();
            //Act
            var isDeleted = await fakeStoryController.DeleteStoryAsync(fakeStory.Id);
            //Assert
            isDeleted.Should().NotBeNull();
            isDeleted.Should().BeOfType<NoContentResult>();
            //newStory.Should().BeOfType<ActionResult<Response<Story>>>();
            var isDeletedObject = (NoContentResult)isDeleted;
            //newStoryObject.Value.Should().BeOfType<ActionResult<Response<Story>>>();
            isDeletedObject.StatusCode.Should().Be(204);
        }
        [Fact]
        public async void DeleteStoryAsync_WithInvalidParameter_ReturnsBadRequest()
        {
            //Arrange
            var errorMessage = "Can't delete story";
            A.CallTo(() => fakeStoryService.DeleteStoryAsync(fakeStory.Id)).Returns(false);
            //Act
            var newStory = await fakeStoryController.DeleteStoryAsync(fakeStory.Id);
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
            A.CallTo(() => fakeStoryService.GetSearchedStoriesAsync(1, 1, fakeStory.AuthorName)).Returns(fakeStoryList);
            A.CallTo(() => fakeStoryService.CountSearchedStoriesAsync(fakeStory.AuthorName)).Returns(fakeStoryList.Count);
        }
        [Fact]
        public async void GetSearchedStoriesAsync_GetSearchedStoriesAsyncIsCalledOnce()
        {

            //Arrange
            ArrangeValidParameters_GetSearchedStoriesAsync();
            //Act
            var myStoryList = await fakeStoryController.GetSearchedStoriesAsync(fakePaginationFilter, fakeStory.AuthorName);
            //Assert
            A.CallTo(() => fakeStoryService.GetSearchedStoriesAsync(1, 1, fakeStory.AuthorName)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void GetSearchedStoriesAsync_CountSearchedStoriesAsyncIsCalledOnce()
        {

            //Arrange
            ArrangeValidParameters_GetSearchedStoriesAsync();
            //Act
            var myStoryList = await fakeStoryController.GetSearchedStoriesAsync(fakePaginationFilter, fakeStory.AuthorName);
            //Assert
            A.CallTo(() => fakeStoryService.CountSearchedStoriesAsync(fakeStory.AuthorName)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void GetSearchedStoriesAsync_WithValidParameter_ReturnsStoryListCorrectly()
        {
            //Arrange
            ArrangeValidParameters_GetSearchedStoriesAsync();
            var fakePagedReponse = PaginationHelper.CreatePagedReponse<Story>(fakeStoryList, fakePaginationFilter, fakeStoryList.Count);
            //Act
            var myStoryList = await fakeStoryController.GetSearchedStoriesAsync(fakePaginationFilter, fakeStory.AuthorName);
            //Assert
            myStoryList.Should().NotBeNull();
            myStoryList.Should().BeOfType<ActionResult<IEnumerable<Story>>>();
            myStoryList.Result.Should().BeOfType<OkObjectResult>();
            var myStoryListObject = (OkObjectResult)myStoryList.Result;
            myStoryListObject.Value.Should().BeEquivalentTo(fakePagedReponse);
            myStoryListObject.StatusCode.Should().Be(200);
        }

        #endregion

        #region GetStoriesOfAUserAsync
        void ArrangeValidParameters_GetStoriesOfAUserAsync()
        {
            A.CallTo(() => fakeStoryService.GetStoriesOfAUserAsync(1, 1, fakeStory.AuthorName)).Returns(fakeStoryList);
            A.CallTo(() => fakeStoryService.CountStoriesOfAUserAsync(fakeStory.AuthorName)).Returns(fakeStoryList.Count);
        }
        [Fact]
        public async void GetStoriesOfAUserAsync_GetStoriesOfAUserAsyncAsyncIsCalledOnce()
        {

            //Arrange
            ArrangeValidParameters_GetStoriesOfAUserAsync();
            //Act
            var myStoryList = await fakeStoryController.GetStoriesOfAUserAsync(fakePaginationFilter, fakeStory.AuthorName);
            //Assert
            A.CallTo(() => fakeStoryService.GetStoriesOfAUserAsync(1, 1, fakeStory.AuthorName)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void GetStoriesOfAUserAsync_GetStoriesOfAUserAsyncIsCalledOnce()
        {

            //Arrange
            ArrangeValidParameters_GetStoriesOfAUserAsync();
            //Act
            var myStoryList = await fakeStoryController.GetStoriesOfAUserAsync(fakePaginationFilter, fakeStory.AuthorName);
            //Assert
            A.CallTo(() => fakeStoryService.CountStoriesOfAUserAsync(fakeStory.AuthorName)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void GetStoriesOfAUserAsync_WithValidParameter_ReturnsStoryListCorrectly()
        {
            //Arrange
            ArrangeValidParameters_GetStoriesOfAUserAsync();
            var fakePagedReponse = PaginationHelper.CreatePagedReponse<Story>(fakeStoryList, fakePaginationFilter, fakeStoryList.Count);
            //Act
            var myStoryList = await fakeStoryController.GetStoriesOfAUserAsync(fakePaginationFilter, fakeStory.AuthorName);
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
