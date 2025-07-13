using DevBoard.Application.Auth;
using DevBoard.Domain.User.Entities;
using DevBoard.Domain.User.ValueObjects;
using DevBoard.Infrastructure.Auth.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;

namespace DevBoard.Infrastructure.Tests.Auth.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IOptions<LockoutOptions> _lockoutOptions;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();

            _lockoutOptions = Options.Create(new LockoutOptions
            {
                MaxFailedAccessAttempts = 3,
                DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15)
            });

            _authService = new AuthService(_userRepositoryMock.Object, _lockoutOptions);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnUser_WhenPasswordIsCorrect()
        {
            // Arrange
            var email = EmailAddress.Create("user@example.com");
            var password = Password.Create("Password1!");

            var user = new UserEnt("Test User", email);
            var hasher = new PasswordHasher<UserEnt>();
            user.SetPasswordHash(hasher.HashPassword(user, password.Value));

            _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(email)).ReturnsAsync(user);

            // Act
            var result = await _authService.LoginAsync(email, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result!.Id);

            _userRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenPasswordIsIncorrect()
        {
            // Arrange
            var email = EmailAddress.Create("user@example.com");
            var correctPassword = Password.Create("Password1!");
            var wrongPassword = Password.Create("WrongPassword1!");

            var user = new UserEnt("Test User", email);
            var hasher = new PasswordHasher<UserEnt>();
            user.SetPasswordHash(hasher.HashPassword(user, correctPassword.Value));

            _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(email)).ReturnsAsync(user);

            // Act
            var result = await _authService.LoginAsync(email, wrongPassword);

            // Assert
            Assert.Null(result);

            _userRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once); // failed attempt registered
        }

        [Fact]
        public async Task LoginAsync_ShouldLockOutUser_AfterMaxFailedAttempts()
        {
            // Arrange
            var email = EmailAddress.Create("user@example.com");
            var correctPassword = Password.Create("Password1!");
            var wrongPassword = Password.Create("WrongPassword1!");

            var user = new UserEnt("Test User", email);
            var hasher = new PasswordHasher<UserEnt>();
            user.SetPasswordHash(hasher.HashPassword(user, correctPassword.Value));

            // Always return same user
            _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(email)).ReturnsAsync(user);

            // Simulate MaxFailedAccessAttempts failed attempts
            for (int i = 0; i < _lockoutOptions.Value.MaxFailedAccessAttempts; i++)
            {
                var result = await _authService.LoginAsync(email, wrongPassword);
                Assert.Null(result);
            }

            // Now user should be locked out — attempt login again and expect exception
            var ex = await Assert.ThrowsAsync<Exception>(() => _authService.LoginAsync(email, correctPassword));

            Assert.Contains("Account locked", ex.Message);

            _userRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Exactly(_lockoutOptions.Value.MaxFailedAccessAttempts + 0)); // failed attempts triggered saves
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var email = EmailAddress.Create("nonexistent@example.com");
            var password = Password.Create("Password1!");

            _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(email)).ReturnsAsync((UserEnt?)null);

            // Act
            var result = await _authService.LoginAsync(email, password);

            // Assert
            Assert.Null(result);

            _userRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_ShouldRegisterUser_WhenEmailDoesNotExist()
        {
            // Arrange
            var name = "New User";
            var email = EmailAddress.Create("newuser@example.com");
            var password = Password.Create("Password1!");

            _userRepositoryMock.Setup(r => r.UserEmailExistsAsync(email)).ReturnsAsync(false);
            _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<UserEnt>())).Returns(Task.CompletedTask);
            _userRepositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _authService.RegisterAsync(name, email, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(name, result!.Name);
            Assert.Equal(email, result.Email);
            Assert.False(string.IsNullOrWhiteSpace(result.PasswordHash));

            _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<UserEnt>()), Times.Once);
            _userRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowException_WhenEmailAlreadyExists()
        {
            // Arrange
            var name = "Existing User";
            var email = EmailAddress.Create("existing@example.com");
            var password = Password.Create("Password1!");

            _userRepositoryMock.Setup(r => r.UserEmailExistsAsync(email)).ReturnsAsync(true);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _authService.RegisterAsync(name, email, password));
            Assert.Equal("User already exists", ex.Message);

            _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<UserEnt>()), Times.Never);
            _userRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}
