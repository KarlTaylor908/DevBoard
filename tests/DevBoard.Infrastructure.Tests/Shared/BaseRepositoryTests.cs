using DevBoard.Domain.User.Entities;
using DevBoard.Domain.User.ValueObjects;
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

        [Fact]
        public async Task Get_ShouldReturnUsers_WhenTheyExist()
        {
            // Arrange
            using var dbContext = CreateInMemoryDbContext();
            dbContext.Users.AddRange(
                new UserEnt ("test1", EmailAddress.Create("test1@example.com")),
                new UserEnt ("test2", EmailAddress.Create("test2@example.com"))
            );
            await dbContext.SaveChangesAsync();

            var repo = new BaseRepository<UserEnt>(dbContext);

            // Act
            var result = await repo.GetAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, u => u.Email is not null && u.Email.Value == "test1@example.com");
        }

        [Fact]
        public async Task Get_ShouldReturnEmptyList_WhenNoUsersExist()
        {
            // Arrange
            using var dbContext = CreateInMemoryDbContext();
            var repo = new BaseRepository<UserEnt>(dbContext);

            // Act
            var result = await repo.GetAsync();

            // Assert
            Assert.Empty(result);
        }
    }
}