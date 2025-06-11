using DevBoard.Application.Auth;
using DevBoard.Application.Auth.UseCases;
using DevBoard.Domain.Auth.Entities;
using DevBoard.Domain.Auth.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Application.Tests.Auth
{
    public class RegisterAsyncTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly RegisterAsync _registerAsync;

        public RegisterAsyncTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _registerAsync = new RegisterAsync(_authServiceMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnUserEnt_WhenRegistrationSucceeds()
        {
            // Arrange
            var name = "John Doe";
            var email = EmailAddress.Create("john@example.com");
            var password = Password.Create("Password1!");

            var expectedUser = new UserEnt(name, email);

            _authServiceMock
                .Setup(x => x.RegisterAsync(name, email, password))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _registerAsync.ExecuteAsync(name, email, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser, result);

            _authServiceMock.Verify(x => x.RegisterAsync(name, email, password), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnNull_WhenRegistrationFails()
        {
            // Arrange
            var name = "Jane Doe";
            var email = EmailAddress.Create("jane@example.com");
            var password = Password.Create("Password1!");

            _authServiceMock
                .Setup(x => x.RegisterAsync(name, email, password))
                .ReturnsAsync((UserEnt?)null);

            // Act
            var result = await _registerAsync.ExecuteAsync(name, email, password);

            // Assert
            Assert.Null(result);

            _authServiceMock.Verify(x => x.RegisterAsync(name, email, password), Times.Once);
        }
    }
}
