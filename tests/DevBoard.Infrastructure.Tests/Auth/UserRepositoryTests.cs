using DevBoard.Domain.User.Entities;
using DevBoard.Domain.User.ValueObjects;
using DevBoard.Infrastructure.Data;
using DevBoard.infrastructure.Auth;
using Microsoft.EntityFrameworkCore;

namespace DevBoard.Infrastructure.Tests.Auth
{
    public class UserRepositoryTests
    {
        private AppDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldReturn_WhenUserExists()
        {
            // Arrange
            using var dbContext = CreateInMemoryDbContext();
            var repo = new UserRepository(dbContext);

            var email = EmailAddress.Create("test@example.com");
            var user = new UserEnt("Test User", email);

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await repo.GetUserByEmailAsync(email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result!.Id);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.Name, result.Name);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldReturnNull_WhenUserDoesnotExist()
        {
            // Arrange
            using var dbContext = CreateInMemoryDbContext();
            var repo = new UserRepository(dbContext);

            var email = EmailAddress.Create("nonexistent@example.com");

            // Act
            var result = await repo.GetUserByEmailAsync(email);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UserEmailExistsAsync_ShouldReturnTrue_WhenUserExists()
        {
            // Arrange
            using var dbContext = CreateInMemoryDbContext();
            var repo = new UserRepository(dbContext);

            var email = EmailAddress.Create("exists@example.com");
            var user = new UserEnt("Existing User", email);

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            // Act
            var exists = await repo.UserEmailExistsAsync(email);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task UserEmailExistsAsync_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            using var dbContext = CreateInMemoryDbContext();
            var repo = new UserRepository(dbContext);

            var email = EmailAddress.Create("missing@example.com");

            // Act
            var exists = await repo.UserEmailExistsAsync(email);

            // Assert
            Assert.False(exists);
        }
    }
}
