using DevBoard.Application.Auth;
using DevBoard.Application.Auth.UseCases;
using DevBoard.Domain.User.Entities;
using DevBoard.Domain.User.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Application.Tests.Auth.UseCases
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

            var emailAddress = EmailAddress.Create("test@example.com");
            Assert.NotNull(emailAddress);

            var password = Password.Create("Password123!");
            Assert.NotNull(password);

            var expectedUser = new UserEnt(name, emailAddress);

            _authServiceMock
                .Setup(service => service.LoginAsync(emailAddress, password))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _loginAsync.ExecuteAsync(emailAddress, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.Name, result.Name);
            Assert.Equal(expectedUser.Email, result.Email);

            _authServiceMock.Verify(x => x.LoginAsync(emailAddress, password), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_UnsuccessfulLogin_ReturnNull()
        {
            // Arrange

            var emailAddress = EmailAddress.Create("test@example.com");
            Assert.NotNull(emailAddress);

            var password = Password.Create("Password123!");
            Assert.NotNull(password);

            _authServiceMock
                .Setup(service => service.LoginAsync(emailAddress, password))
                .ReturnsAsync((UserEnt?)null);

            // Act
            var result = await _loginAsync.ExecuteAsync(emailAddress, password);

            // Assert
            Assert.Null(result);
            _authServiceMock.Verify(x => x.LoginAsync(emailAddress, password), Times.Once);

        }
    }
}
