using DevBoard.Domain.Auth.Entities;

namespace DevBoard.Domain.Tests.Auth.Entities
{
    public class UserEntTests 
    {
        UserEnt exampleUser = new UserEnt("TestUser", "testuser@example.com");
        int maxAttempts = 5;
        TimeSpan lockoutTimeSpan = TimeSpan.FromMinutes(5);

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
            var email = "testuser@example.com";

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
            exampleUser.ResetFailedAttempts();

            // Act
            var isLockedOut = exampleUser.IsLockedOut();

            // Assert
            Assert.False(isLockedOut);
        }

        [Fact]
        public async Task IsLockedOut_LockoutUntilIsLessThanUtcNow_ReturnFalse()
        {
            // Arrange
            exampleUser.ResetFailedAttempts();

            lockoutTimeSpan = TimeSpan.FromMicroseconds(1);

            for (var i = 0; i < 5; i++)
            {
                exampleUser.RegisterFailedAttempt(maxAttempts, lockoutTimeSpan);
            }

            // Act
            await Task.Delay(5000);
            var isLockedOut = exampleUser.IsLockedOut();

            // Assert
            Assert.False(isLockedOut);

        }

        [Fact]
        public void IsLockedOut_LockoutUntilIsMoreThanUtcNow_ReturnTrue()
        {
            // Arrange
            exampleUser.ResetFailedAttempts();

            for (var i = 0; i < 5; i++)
            {
                exampleUser.RegisterFailedAttempt(maxAttempts, lockoutTimeSpan);
            }

            // Act
            var isLockedOut = exampleUser.IsLockedOut();

            // Assert
            Assert.True(isLockedOut);
        }

        [Fact]
        public void RegisterFailedAttempt_MaxAttemptIsMoreThanFailedLoginAttempts_LockoutUntilIsNull()
        {
            // Arrange
            exampleUser.ResetFailedAttempts();

            // Act
            exampleUser.RegisterFailedAttempt(maxAttempts, lockoutTimeSpan);

            // Assert
            Assert.Equal(1, exampleUser.FailedLoginAttempts);
            Assert.Null(exampleUser.LockoutUntil);
        }

        [Fact]
        public void RegisterFailedAttempt_MaxAttemptIsEqualToFailedLoginAttempts_LockoutUntilIsNotNull()
        {
            // Arrange
            exampleUser.ResetFailedAttempts();

            // Act
            for (var i = 0; i < 5; i++)
            {
                exampleUser.RegisterFailedAttempt(maxAttempts, lockoutTimeSpan);
            }
            
            // Assert
            Assert.Equal(5, exampleUser.FailedLoginAttempts);
            Assert.NotNull(exampleUser.LockoutUntil);
        }

        [Fact]
        public void RegisterFailedAttempt_MaxAttemptIsMoreThanFailedLoginAttempts_LockoutUntilIsNotNull()
        {
            // Arrange
            exampleUser.ResetFailedAttempts();

            // Act
            for (var i = 0; i < 6; i++)
            {
                exampleUser.RegisterFailedAttempt(maxAttempts, lockoutTimeSpan);
            }

            // Assert
            Assert.Equal(6, exampleUser.FailedLoginAttempts);
            Assert.NotNull(exampleUser.LockoutUntil);
        }

        [Fact]
        public void ResetFailedAttempts_FailedLoginAttemptsIsZeroAndLockoutUntilIsNull()
        {
            // Arrange
            for (var i = 0; i < 6; i++)
            {
                exampleUser.RegisterFailedAttempt(maxAttempts, lockoutTimeSpan);
            }

            // Act
            exampleUser.ResetFailedAttempts();

            // Arrange
            Assert.Equal(0, exampleUser.FailedLoginAttempts);
            Assert.Null(exampleUser.LockoutUntil);
        }

        [Fact]
        // This test does not tets the actual hashing, this is done in the auth service
        public void SetPasswordHash_SetsPasswordHash()
        {
            // Arrange
            var password = "password";

            // Act
            exampleUser.SetPasswordHash(password);

            // Assert
            Assert.Equal(password, exampleUser.PasswordHash);
        }
    }
}
