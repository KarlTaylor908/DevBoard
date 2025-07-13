using DevBoard.API.User.Controllers;
using DevBoard.API.User.DTOs;
using DevBoard.Application.User.Interfaces;
using DevBoard.Domain.User.Entities;
using DevBoard.Domain.User.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace DevBoard.API.Tests.User.Controllers
{
    public class UserControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _userController;
        private readonly WebApplicationFactory<Program> _factory;

        public UserControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;

            _userServiceMock = new Mock<IUserService>();
            _userController = new UserController(_userServiceMock.Object);

            var context = new DefaultHttpContext();
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };
        }

        [Fact]
        public async Task GetUsers_ShouldReturn401_WhenNotLoggedIn()
        {
            // Arrange
            HttpClient client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/users/get");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetUsers_ShouldReturn200_WhenIsLoggedIn()
        {
            // Arrange
            var testUsers = new List<UserEnt>
            {
                new UserEnt("test1", EmailAddress.Create("test1@example.com")),
                new UserEnt("test2", EmailAddress.Create("test2@example.com"))
            };

            _userServiceMock.Setup(s => s.GetUsers()).ReturnsAsync(testUsers);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "test1@example.com"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("name", "test1")
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrinciple = new ClaimsPrincipal(identity);

            // Act
            var result = await _userController.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsAssignableFrom<List<UserEnt>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count);
        }

        [Fact]
        public async Task GetUsers_ShouldReturn200_WhenUserExists()
        {
            // Arrange
            var testUsers = new List<UserEnt>
            {
                new UserEnt("test1", EmailAddress.Create("test1@example.com")),
                new UserEnt("test2", EmailAddress.Create("test2@example.com"))
            };

            _userServiceMock.Setup(s => s.GetUsers()).ReturnsAsync(testUsers);

            // Act
            var result = await _userController.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsType<List<UserEnt>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count);
        }

        [Fact]
        public async Task GetUsers_ShouldReturn400_WhenNoUsersExist()
        {
            // Arrange
            _userServiceMock.Setup(s => s.GetUsers()).ReturnsAsync(new List<UserEnt>());

            // Act
            var result = await _userController.GetUsers();

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No users were found.", badRequest.Value);
        }

        [Fact]
        public async Task GetUsers_ShouldReturn400_WhenExceptionIsThrown()
        {
            // Arrange
            _userServiceMock.Setup(s => s.GetUsers()).ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _userController.GetUsers();

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Something went wrong", badRequest.Value);
        }
    }
}
