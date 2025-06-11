using DevBoard.API.Auth.Controllers;
using DevBoard.API.Auth.Requests;
using DevBoard.API.Auth.Responses;
using DevBoard.Application.Auth;
using DevBoard.Application.Auth.Interfaces;
using DevBoard.Domain.Auth.Entities;
using DevBoard.Domain.Auth.ValueObjects;
using DevBoard.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DevBoard.API.Tests.Auth.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly AuthController _authController;

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _jwtServiceMock = new Mock<IJwtService>();

            _authController = new AuthController(_authServiceMock.Object, _jwtServiceMock.Object);
        }

        [Fact]
        public async Task Register_ShouldReturnAuthResponse_WhenRegistrationSucceeds()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Name = "Test User",
                Email = "user@example.com",
                Password = "Password1!"
            };

            var user = new UserEnt(request.Name, EmailAddress.Create(request.Email));

            _authServiceMock.Setup(x => x.RegisterAsync(
                request.Name,
                It.IsAny<EmailAddress>(),
                It.IsAny<Password>()
            )).ReturnsAsync(user);

            _jwtServiceMock.Setup(x => x.GenerateToken(user)).Returns("fake-jwt-token");

            // Act
            var result = await _authController.Register(request);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var authResponse = Assert.IsType<AuthResponse>(jsonResult.Value);

            Assert.Equal(request.Email, authResponse.Email);
            Assert.Equal("fake-jwt-token", authResponse.Token);
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Name = "Test User",
                Email = "user@example.com",
                Password = "Password1!"
            };

            _authServiceMock.Setup(x => x.RegisterAsync(
                request.Name,
                It.IsAny<EmailAddress>(),
                It.IsAny<Password>()
            )).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _authController.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Test exception", badRequestResult.Value);
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenRegisterReturnsNull()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Name = "Test User",
                Email = "user@example.com",
                Password = "Password1!"
            };

            _authServiceMock.Setup(x => x.RegisterAsync(
                request.Name,
                It.IsAny<EmailAddress>(),
                It.IsAny<Password>()
            )).ReturnsAsync((UserEnt?)null);

            // Act
            var result = await _authController.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid Credentials", badRequestResult.Value);
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenRegisterReturnsNullEmail()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Name = "Test User",
                Email = "user@example.com",
                Password = "Password1!"
            };

            _authServiceMock.Setup(x => x.RegisterAsync(
                request.Name,
                It.IsAny<EmailAddress>(),
                It.IsAny<Password>()
            )).ReturnsAsync(new UserEnt(request.Name, null));

            // Act
            var result = await _authController.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid Credentials", badRequestResult.Value);
        }

        [Fact]
        public async Task Login_ShouldReturnAuthResponse_WhenLoginSucceeds()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "user@example.com",
                Password = "Password1!"
            };

            var user = new UserEnt("Test User", EmailAddress.Create(request.Email));

            _authServiceMock.Setup(x => x.LoginAsync(
                It.IsAny<EmailAddress>(),
                It.IsAny<Password>()
            )).ReturnsAsync(user);

            _jwtServiceMock.Setup(x => x.GenerateToken(user)).Returns("fake-jwt-token");

            // Act
            var result = await _authController.Login(request);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var authResponse = Assert.IsType<AuthResponse>(jsonResult.Value);

            Assert.Equal(request.Email, authResponse.Email);
            Assert.Equal("fake-jwt-token", authResponse.Token);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenLoginFails()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "user@example.com",
                Password = "Password1!"
            };

            _authServiceMock.Setup(x => x.LoginAsync(
                It.IsAny<EmailAddress>(),
                It.IsAny<Password>()
            )).ReturnsAsync((UserEnt?)null);

            // Act
            var result = await _authController.Login(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Login Failed", unauthorizedResult.Value);
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenInvalidEmail()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "invalid-email",
                Password = "Password!123"
            };

            // Act
            var result = await _authController.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Login Failed", badRequestResult.Value);
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenInvalidPassword()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "testuser@example.com",
                Password = "short"
            };

            // Act
            var result = await _authController.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Login Failed", badRequestResult.Value);
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "user@example.com",
                Password = "Password1!"
            };

            _authServiceMock.Setup(x => x.LoginAsync(
                It.IsAny<EmailAddress>(),
                It.IsAny<Password>()
            )).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _authController.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Test exception", badRequestResult.Value);
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenLoginReturnsNull()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "user@example.com",
                Password = "Password1!"
            };

            _authServiceMock.Setup(x => x.LoginAsync(
                It.IsAny<EmailAddress>(),
                It.IsAny<Password>()
            )).ReturnsAsync((UserEnt?)null);

            // Act
            var result = await _authController.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Login Failed", badRequestResult.Value);
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenLoginReturnsNullEmail()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "user@example.com",
                Password = "Password1!"
            };

            _authServiceMock.Setup(x => x.LoginAsync(
                It.IsAny<EmailAddress>(),
                It.IsAny<Password>()
            )).ReturnsAsync(new UserEnt(It.IsAny<string>(), null));

            // Act
            var result = await _authController.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Login Failed", badRequestResult.Value);
        }
    }
}
