using DevBoard.Application.Auth.Interfaces;
using DevBoard.Domain.Auth.Entities;
using DevBoard.Domain.Auth.ValueObjects;
using DevBoard.Infrastructure.Auth;
using DevBoard.Infrastructure.Auth.Services;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace DevBoard.Infrastructure.Tests.Auth.Services
{
    public class JwtServiceTests
    {
        private IJwtService CreateJwtService()
        {
            var jwtSettings = new JwtSettings
            {
                Key = "SuperSecretTestKeyThatIsLongEnough123!", // Must be long enough for HMACSHA256
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                DurationInMinutes = 60
            };

            return new JwtService(Options.Create(jwtSettings));
        }

        [Fact]
        public void GenerateToken_ShouldReturnValidToken_WithExpectedClaims()
        {
            // Arrange
            var jwtService = CreateJwtService();

            var user = new UserEnt("Test User", EmailAddress.Create("user@example.com"));

            // Act
            var tokenString = jwtService.GenerateToken(user);

            // Assert basic checks
            Assert.False(string.IsNullOrWhiteSpace(tokenString));

            // Validate and read the token
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);

            // Validate claims
            var subClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            var uidClaim = token.Claims.FirstOrDefault(c => c.Type == "uid");
            var nameClaim = token.Claims.FirstOrDefault(c => c.Type == "name");
            var jtiClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);

            Assert.NotNull(subClaim);
            Assert.Equal(user.Email!.Value, subClaim!.Value);

            Assert.NotNull(uidClaim);
            Assert.Equal(user.Id.ToString(), uidClaim!.Value);

            Assert.NotNull(nameClaim);
            Assert.Equal(user.Name, nameClaim!.Value);

            Assert.NotNull(jtiClaim);
            Assert.False(string.IsNullOrWhiteSpace(jtiClaim!.Value));

            // Validate token properties
            Assert.Equal("TestIssuer", token.Issuer);
            Assert.Equal("TestAudience", token.Audiences.First());
        }

        [Fact]
        public void GenerateToken_ShouldThrowArgumentNullException_WhenUserEmailIsNull()
        {
            // Arrange
            var jwtService = CreateJwtService();

            var user = new UserEnt("Test User", null!); // Simulate invalid UserEnt

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => jwtService.GenerateToken(user));
        }
    }
}
