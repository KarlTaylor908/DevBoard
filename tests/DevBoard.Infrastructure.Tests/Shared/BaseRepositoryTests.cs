using DevBoard.Domain.Auth.Entities;
using DevBoard.Domain.Auth.ValueObjects;
using DevBoard.Infrastructure.Data;
using DevBoard.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace DevBoard.Infrastructure.Tests.Shared
{
    public class BaseRepositoryTests
    {
        private AppDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unique DB per test
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_And_SaveChangesAsync_ShouldPersistEntity()
        {
            // Arrange
            using var dbContext = CreateInMemoryDbContext();
            var repo = new BaseRepository<UserEnt>(dbContext);

            var email = EmailAddress.Create("user@example.com");
            var user = new UserEnt("Test User", email);

            // Act
            await repo.AddAsync(user);
            await repo.SaveChangesAsync();

            // Assert - directly check the DbSet
            var savedUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

            Assert.NotNull(savedUser);
            Assert.Equal(user.Id, savedUser!.Id);
            Assert.Equal(user.Name, savedUser.Name);
            Assert.Equal(user.Email, savedUser.Email);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnTrue_WhenEntityExists()
        {
            // Arrange
            using var dbContext = CreateInMemoryDbContext();
            var repo = new BaseRepository<UserEnt>(dbContext);

            var email = EmailAddress.Create("user@example.com");
            var user = new UserEnt("Test User", email);

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            // Act
            var exists = await repo.ExistsAsync(user.Id);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnFalse_WhenEntityDoesNotExist()
        {
            // Arrange
            using var dbContext = CreateInMemoryDbContext();
            var repo = new BaseRepository<UserEnt>(dbContext);

            // Act
            var exists = await repo.ExistsAsync(Guid.NewGuid()); // random ID

            // Assert
            Assert.False(exists);
        }
    }
}