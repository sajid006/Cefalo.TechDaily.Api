using Cefalo.TechDaily.Database.Context;
using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Repository.Contracts;
using Cefalo.TechDaily.Repository.Repositories;
using Cefalo.TechDaily.Repository.UnitTests.FakeData;
using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Repository.UnitTests.RepositoryUnitTests
{
    public class StoryRepositoryUnitTests
    {
        private readonly DataContext fakeDataContext;
        private readonly IStoryRepository fakeStoryRepository;
        private readonly FakeStoryData fakeStoryData;
        private readonly List<Story> fakeStoryList;
        private readonly Story fakeStory, fakeStory3, fakeStory4;
        private readonly int fakePageNumber, fakePageSize;
        public StoryRepositoryUnitTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(databaseName: "fakeDataContext").Options;
            fakeDataContext = new DataContext(options);
            fakeStoryRepository = new StoryRepository(fakeDataContext);
            fakeStoryData = A.Fake<FakeStoryData>();
            fakeStoryList = fakeStoryData.fakeStoryList;
            fakeStory = fakeStoryData.fakeStory;
            fakeStory3 = fakeStoryData.fakeStory3;
            fakeStory4 = fakeStoryData.fakeStory4;
            fakePageNumber = 1;
            fakePageSize = 2;
        }
        private async void ArrangeValidParameters()
        {
            fakeDataContext.Stories.RemoveRange(fakeDataContext.Stories);
            foreach (Story fakeStory in fakeStoryList)
            {
                fakeDataContext.Stories.Add(fakeStory);
                await fakeDataContext.SaveChangesAsync();
            }
        }
        [Fact]
        public async void GetStoriesAsync_WithValidParameter_ReturnsStoryListCorrectly()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var storyList = await fakeStoryRepository.GetStoriesAsync(fakePageNumber, fakePageSize);
            //Assert
            storyList.Should().BeEquivalentTo(fakeStoryList);
            storyList.Should().BeOfType<List<Story>>();

        }
        [Fact]
        public async void GetStoryByIdAsync_WithValidParameter_ReturnsStoryCorrectly()
        {
            //Arrange
            fakeDataContext.Stories.Add(fakeStory3);
            await fakeDataContext.SaveChangesAsync();
            //Act
            var story = await fakeStoryRepository.GetStoryByIdAsync(fakeStory3.Id);
            //Assert
            story.Should().BeEquivalentTo(fakeStory3);
        }
        [Fact]
        public async void GetStoryByIdAsync_WithInvalidParameter_ReturnsNullStory()
        {
            //Arrange
            //Act
            var story = await fakeStoryRepository.GetStoryByIdAsync(300);
            //Assert
            story.Should().BeNull();
        }
        [Fact]
        public async void PostStoryAsync_WithValidParameter_ReturnsCreatedStory()
        {
            //Arrange
            //fakeDataContext.Stories.RemoveRange(fakeDataContext.Stories);
            //Act
            var story = await fakeStoryRepository.PostStoryAsync(fakeStory4);
            //Assert
            story.Should().NotBeNull();
            story.Should().BeEquivalentTo(fakeStory4);
        }
        [Fact]
        public async void UpdateStoryAsync_WithValidParameter_ReturnsUpdatedStory()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var story = await fakeStoryRepository.UpdateStoryAsync(fakeStory.Id, fakeStory);
            //Assert
            story.Should().NotBeNull();
            story.Should().BeEquivalentTo(fakeStory);
        }
        [Fact]
        public async void UpdateStoryAsync_WithInvalidParameters_ReturnsNull()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var story = await fakeStoryRepository.UpdateStoryAsync(300, fakeStory);
            //Assert
            story.Should().BeNull();
        }
        [Fact]
        public async void DeleteStoryAsync_WithValidParameter_ReturnsTrue()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var deleted = await fakeStoryRepository.DeleteStoryAsync(fakeStory.Id);
            //Assert
            deleted.Should().BeTrue();
        }
        [Fact]
        public async void GetStoriesOfAUserAsync_WithValidParameter_ReturnsCorrectStoryList()
        {
            //Arrange
            ArrangeValidParameters();
            List<Story> tempStoryList;
            tempStoryList = new List<Story>();
            tempStoryList.Add(fakeStory);
            //Act
            var storyList = await fakeStoryRepository.GetStoriesOfAUserAsync(fakePageNumber,fakePageSize, fakeStory.AuthorName);
            //Assert
            storyList.Should().BeEquivalentTo(tempStoryList);
        }
        [Fact]
        public async void GetSearchedStoriesAsync_WithValidParameter_ReturnsCorrectStoryListBasedOnAuthorName()
        {
            //Arrange
            ArrangeValidParameters();
            List<Story> tempStoryList;
            tempStoryList = new List<Story>();
            tempStoryList.Add(fakeStory);
            //Act
            var storyList = await fakeStoryRepository.GetSearchedStoriesAsync(fakePageNumber, fakePageSize, fakeStory.AuthorName);
            //Assert
            storyList.Should().BeEquivalentTo(tempStoryList);
        }
        [Fact]
        public async void GetSearchedStoriesAsync_WithValidParameter_ReturnsCorrectStoryListBasedOnTitle()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var storyList = await fakeStoryRepository.GetSearchedStoriesAsync(fakePageNumber, fakePageSize, fakeStory.Title);
            //Assert
            storyList.Should().BeEquivalentTo(fakeStoryList);
        }
        [Fact]
        public async void CountStoriesAsync_ReturnsCountOfStories()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var storyList = await fakeStoryRepository.CountStoriesAsync();
            //Assert
            storyList.Should().Be(fakeStoryList.Count);
        }
        [Fact]
        public async void CountStoriesOfAUserAsync_WithValidParameters_ReturnsCountOfStoriesOfAUserAsync()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var storyList = await fakeStoryRepository.CountStoriesOfAUserAsync(fakeStoryList[0].AuthorName);
            //Assert
            storyList.Should().Be(1);
        }
        [Fact]
        public async void CountSearchedStoriesAsync_WithValidParameters_ReturnsCountOfStoriesBasedOnAuthorName()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var storyList = await fakeStoryRepository.CountSearchedStoriesAsync(fakeStoryList[0].AuthorName);
            //Assert
            storyList.Should().Be(1);
        }
        [Fact]
        public async void CountSearchedStoriesAsync_WithValidParameters_ReturnsCountOfStoriesBasedOnTitle()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var storyList = await fakeStoryRepository.CountSearchedStoriesAsync(fakeStoryList[0].Title);
            //Assert
            storyList.Should().Be(fakeStoryList.Count);
        }
        
    }
}
