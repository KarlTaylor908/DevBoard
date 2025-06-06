using DevBoard.Application.Auth;
using DevBoard.Application.Auth.UseCases;
using DevBoard.Domain.Auth;
using DevBoard.Domain.Auth.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Application.Tests.Auth
{
    public class LoginAsyncTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly LoginAsync _loginAsync;

        public LoginAsyncTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _loginAsync = new LoginAsync(_authServiceMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_SucessfullLogin_ReturnsUser()
        {
            // Arrange
            var name = "testuser";

            var emailCreated = EmailAddress.TryCreate("test@example.com", out var emailAddress);
            Assert.True(emailCreated);
            Assert.NotNull(emailAddress);

            var password = "password";
            var expectedUser = new UserEnt(name, emailAddress!);

            _authServiceMock
                .Setup(service => service.LoginAsync(emailAddress!, password))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _loginAsync.ExecuteAsync(emailAddress!, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.Name, result.Name);
            Assert.Equal(expectedUser.Email, result.Email);
        }

        [Fact]
        public async Task ExecuteAsync_UnsuccessfulLogin_ReturnNull()
        {
            // Arrange

            var emailCreated = EmailAddress.TryCreate("test@example.com", out var emailAddress);
            Assert.True(emailCreated);
            Assert.NotNull(emailAddress);

            var password = "password";

            _authServiceMock
                .Setup(service => service.LoginAsync(emailAddress!, password))
                .ReturnsAsync((UserEnt?)null);

            // Act
            var result = await _loginAsync.ExecuteAsync(emailAddress!, password);

            // Assert
            Assert.Null(result);
        }
    }
}
