using DevBoard.API.Auth.Controllers;
using DevBoard.API.Auth.Requests;
using DevBoard.API.Auth.Responses;
using DevBoard.Application.Auth;
using DevBoard.Application.Auth.Interfaces;
using DevBoard.Domain.User.Entities;
using DevBoard.Domain.User.ValueObjects;
using DevBoard.Infrastructure.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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

            IOptions<JwtSettings> jwtOptions = Options.Create(new JwtSettings
            {
                Key = "c35ca0c377fac9e8331f7c45872d6875b2b9c5473e0ecdc497efcd809306db03811ca77c327559be71cf493a0bbf613005e1ba8fadcd9fd53efca222317e276cd6e12d01a9194f0e6cc8f859d70e329c3e895c58b45378b62bf90819874a86d897ef0567407b96bec986c7c69849e7ed2aa20ebcb4544414c6d307fbb78b1a0405172888b59604dcc4311f957f0038d09894048b21e915e88bff00828543ba23e0d9505aa31d747b44943d6b59faaf02e464fcfb5c2737783d7e2ae4524d1a6fd58c400c3957e797b7b5da90f448ceb486f47d0973f3d1d10e6b98f3de23f2ea7f2fddcf48088f1c74bfc96df77226c9efb44bf3aeddbc14163210f66ac046d3",
                Issuer = "DevBoard.API",
                Audience = "DevBoard.API",
                DurationInMinutes = 60
            });

            _authController = new AuthController(_authServiceMock.Object, _jwtServiceMock.Object, jwtOptions);
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
        public async Task Login_ShouldSetJwtTokenCookie_WhenLoginSucceeds()
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

            var responseCookiesMock = new Mock<IResponseCookies>();
            var responseMock = new Mock<HttpResponse>();
            responseMock.SetupGet(r => r.Cookies).Returns(responseCookiesMock.Object);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(c => c.Response).Returns(responseMock.Object);

            _authController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContextMock.Object
            };

            // Act
            var result = await _authController.Login(request);

            // Assert
            responseCookiesMock.Verify(cookies => cookies.Append(
                "jwt_token",
                "fake-jwt-token",
                It.Is<CookieOptions>(options =>
                    options.HttpOnly &&
                    options.Secure &&
                    options.SameSite == SameSiteMode.Strict &&
                    options.Expires.HasValue
                )
            ), Times.Once);
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
