using DevBoard.Application.Auth;
using DevBoard.Domain.User.Entities;
using DevBoard.Domain.User.ValueObjects;
using DevBoard.Infrastructure.User.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Infrastructure.Tests.User.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnList_WhenUsersExist()
        {
            // Arrange
            var users = new List<UserEnt>
            {
                new UserEnt("test1", EmailAddress.Create("test1@example.com")),
                new UserEnt("test2", EmailAddress.Create("test2@example.com"))
            };

            _userRepositoryMock.Setup(repo => repo.GetAsync()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetUsers();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(EmailAddress.Create("test1@example.com"), result.First().Email);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnEmptyList_WhenNoUsersExist()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetAsync()).ReturnsAsync(new List<UserEnt>());

            // Act
            var result = await _userService.GetUsers();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUsers_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetAsync()).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.GetUsers());
            Assert.Equal("Database error", exception.Message);
        }

    }
}
