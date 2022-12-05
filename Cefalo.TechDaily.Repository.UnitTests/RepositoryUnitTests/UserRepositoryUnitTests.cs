using Cefalo.TechDaily.Database.Context;
using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Repository.Contracts;
using Cefalo.TechDaily.Repository.Repositories;
using Cefalo.TechDaily.Repository.UnitTests.FakeData;
using Cefalo.TechDaily.Service.Dto;
using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Repository.UnitTests.RepositoryUnitTests
{
    public class UserRepositoryUnitTests
    {
        private readonly DataContext fakeDataContext;
        private readonly IUserRepository fakeUserRepository;
        private readonly FakeUserData fakeUserData;
        private readonly List<User> fakeUserList;
        private readonly User fakeUser, fakeUser3, fakeUser4;
        public UserRepositoryUnitTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(databaseName: "fakeDataContext").Options;
            fakeDataContext = new DataContext(options);
            fakeUserRepository = new UserRepository(fakeDataContext);
            fakeUserData = A.Fake<FakeUserData>();
            fakeUserList = fakeUserData.fakeUserList;
            fakeUser = fakeUserData.fakeUser;
            fakeUser3 = fakeUserData.fakeUser3;
            fakeUser4 = fakeUserData.fakeUser4;
        }
        private async void ArrangeValidParameters()
        {
            fakeDataContext.Users.RemoveRange(fakeDataContext.Users);
            foreach (User fakeUser in fakeUserList)
            {
                fakeDataContext.Users.Add(fakeUser);
                await fakeDataContext.SaveChangesAsync();
            }
        }
        [Fact]
        public async void GetUsersAsync_WithValidParameter_ReturnsUserListCorrectly()
        {
            //Arrange
            //test fixture
            ArrangeValidParameters();
            //Act
            var userList = await fakeUserRepository.GetUsersAsync();
            //Assert
            userList.Should().BeEquivalentTo(fakeUserList);
            userList.Should().BeOfType<List<User>>();

        }
        [Fact]
        public async void GetUserByUsernameAsync_WithValidParameter_ReturnsUserCorrectly()
        {
            //Arrange
            fakeDataContext.Users.Add(fakeUser3);
            await fakeDataContext.SaveChangesAsync();
            //Act
            var user = await fakeUserRepository.GetUserByUsernameAsync(fakeUser3.Username);
            //Assert
            user.Should().BeEquivalentTo(fakeUser3);
            //user.Should().BeOfType<User>();
        }
        [Fact]
        public async void GetUserByUsernameAsync_WithInvalidParameter_ReturnsNullUser()
        {
            //Arrange
            //Act
            var user = await fakeUserRepository.GetUserByUsernameAsync("nulluser");
            //Assert
            user.Should().BeNull();
        }
        [Fact]
        public async void PostUserAsync_WithValidParameter_ReturnsCreatedUser()
        {
            //Arrange
            //fakeDataContext.Users.RemoveRange(fakeDataContext.Users);
            //Act
            var user = await fakeUserRepository.PostUserAsync(fakeUser4);
            //Assert
            user.Should().NotBeNull();
            user.Should().BeEquivalentTo(fakeUser4);
        }
        [Fact]
        public async void UpdateUserAsync_WithValidParameter_ReturnsUpdatedUser()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var user = await fakeUserRepository.UpdateUserAsync(fakeUser.Username, fakeUser);
            //Assert
            user.Should().NotBeNull();
            user.Should().BeEquivalentTo(fakeUser);
        }
        [Fact]
        public async void UpdateUserAsync_WithValidParameterButPasswordHashIsNull_ReturnsUpdatedUser()
        {
            //Arrange
            ArrangeValidParameters();
            User tempUser = A.Fake<User>(x => x.WithArgumentsForConstructor(() => new User()));
            tempUser.Username = "sajid1";
            tempUser.Name = "Sajid Hasan";
            tempUser.Email = "sajid1@gmail.com";
            tempUser.PasswordHash = null;
            tempUser.PasswordSalt = null;
            tempUser.UpdatedAt = FakeUserData.TrimMilliseconds(DateTime.UtcNow);
            tempUser.CreatedAt = FakeUserData.TrimMilliseconds(DateTime.UtcNow);
            tempUser.PasswordModifiedAt = FakeUserData.TrimMilliseconds(DateTime.UtcNow);
            //Act
            var user = await fakeUserRepository.UpdateUserAsync(tempUser.Username, tempUser);
            //Assert
            user.Should().NotBeNull();
            user.Should().NotBeEquivalentTo(tempUser);
            user.Should().BeEquivalentTo(fakeUser);
        }
        [Fact]
        public async void DeleteUserAsync_WithValidParameter_ReturnsTrue()
        {
            //Arrange
            ArrangeValidParameters();
            //Act
            var deleted = await fakeUserRepository.DeleteUserAsync(fakeUser.Username);
            //Assert
            deleted.Should().BeTrue();
        }
    }
}
