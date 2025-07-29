using DevBoard.Domain.User.Entities;
using DevBoard.Domain.User.ValueObjects;

namespace DevBoard.Domain.Tests.User.Entities
{
    public class UserEntTests 
    {
        private EmailAddress _emailAddress = EmailAddress.Create("testuser@example.com");
        private UserEnt _exampleUser;
        private int _maxAttempts = 5;
        private TimeSpan _lockoutTimeSpan = TimeSpan.FromMinutes(5);

        public UserEntTests()
        {
            _exampleUser = new UserEnt("TestUser", _emailAddress);
        }

        [Fact]
        public void Constructor_Default_SetInitialValuesCorrectly()
        {
            // Act
            var user = new UserEnt();

            // Assert
            Assert.NotEqual(Guid.Empty, user.Id);
        }

        [Fact]
        public void Constructor_SetValues_SetPropertiesCorrectly()
        {
            // Arrange
            var name = "TestUser";
            var email = EmailAddress.Create("testuser@example.com");

            // Act
            var user = new UserEnt
            (
                name,
                email
            );

            // Assert
            Assert.NotEqual(Guid.Empty, user.Id);
            Assert.Equal(name, user.Name);
            Assert.Equal(email, user.Email);
        }

        [Fact]
        public void IsLockedout_LockedoutUntilHasNoValue_ReturnFalse()
        {
            // Arrange
            _exampleUser.ResetFailedAttempts();

            // Act
            var isLockedOut = _exampleUser.IsLockedOut();

            // Assert
            Assert.False(isLockedOut);
        }

        [Fact]
        public async Task IsLockedOut_LockoutUntilIsLessThanUtcNow_ReturnFalse()
        {
            // Arrange
            _exampleUser.ResetFailedAttempts();

            _lockoutTimeSpan = TimeSpan.FromMicroseconds(1);

            for (var i = 0; i < 5; i++)
            {
                _exampleUser.RegisterFailedAttempt(_maxAttempts, _lockoutTimeSpan);
            }

            // Act
            await Task.Delay(5000);
            var isLockedOut = _exampleUser.IsLockedOut();

            // Assert
            Assert.False(isLockedOut);

        }

        [Fact]
        public void IsLockedOut_LockoutUntilIsMoreThanUtcNow_ReturnTrue()
        {
            // Arrange
            _exampleUser.ResetFailedAttempts();

            for (var i = 0; i < 5; i++)
            {
                _exampleUser.RegisterFailedAttempt(_maxAttempts, _lockoutTimeSpan);
            }

            // Act
            var isLockedOut = _exampleUser.IsLockedOut();

            // Assert
            Assert.True(isLockedOut);
        }

        [Fact]
        public void RegisterFailedAttempt_MaxAttemptIsMoreThanFailedLoginAttempts_LockoutUntilIsNull()
        {
            // Arrange
            _exampleUser.ResetFailedAttempts();

            // Act
            _exampleUser.RegisterFailedAttempt(_maxAttempts, _lockoutTimeSpan);

            // Assert
            Assert.Equal(1, _exampleUser.FailedLoginAttempts);
            Assert.Null(_exampleUser.LockoutUntil);
        }

        [Fact]
        public void RegisterFailedAttempt_MaxAttemptIsEqualToFailedLoginAttempts_LockoutUntilIsNotNull()
        {
            // Arrange
            _exampleUser.ResetFailedAttempts();

            // Act
            for (var i = 0; i < 5; i++)
            {
                _exampleUser.RegisterFailedAttempt(_maxAttempts, _lockoutTimeSpan);
            }
            
            // Assert
            Assert.Equal(5, _exampleUser.FailedLoginAttempts);
            Assert.NotNull(_exampleUser.LockoutUntil);
        }

        [Fact]
        public void RegisterFailedAttempt_MaxAttemptIsMoreThanFailedLoginAttempts_LockoutUntilIsNotNull()
        {
            // Arrange
            _exampleUser.ResetFailedAttempts();

            // Act
            for (var i = 0; i < 6; i++)
            {
                _exampleUser.RegisterFailedAttempt(_maxAttempts, _lockoutTimeSpan);
            }

            // Assert
            Assert.Equal(6, _exampleUser.FailedLoginAttempts);
            Assert.NotNull(_exampleUser.LockoutUntil);
        }

        [Fact]
        public void ResetFailedAttempts_FailedLoginAttemptsIsZeroAndLockoutUntilIsNull()
        {
            // Arrange
            for (var i = 0; i < 6; i++)
            {
                _exampleUser.RegisterFailedAttempt(_maxAttempts, _lockoutTimeSpan);
            }

            // Act
            _exampleUser.ResetFailedAttempts();

            // Arrange
            Assert.Equal(0, _exampleUser.FailedLoginAttempts);
            Assert.Null(_exampleUser.LockoutUntil);
        }

        [Fact]
        // This test does not tets the actual hashing, this is done in the auth service
        public void SetPasswordHash_SetsPasswordHash()
        {
            // Arrange
            var password = "password";

            // Act
            _exampleUser.SetPasswordHash(password);

            // Assert
            Assert.Equal(password, _exampleUser.PasswordHash);
        }
    }
}
