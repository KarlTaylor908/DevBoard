using DevBoard.Domain.Auth.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Domain.Tests.Auth.ValueObjects
{
    public class PasswordTests
    {
        [Theory]
        [InlineData("Aa1!aaaa")]
        [InlineData("P@ssw0rd123")]
        [InlineData("Complex#Pass9")]
        public void Create_ShouldReturnPassword_WhenPasswordIsValid(string validPassword)
        {
            // Act
            var password = Password.Create(validPassword);

            // Assert
            Assert.NotNull(password);
            Assert.Equal(validPassword, password.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Create_ShouldThrowArgumentNullException_WhenPasswordIsNullOrWhitespace(string? invalidPassword)
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Password.Create(invalidPassword!));
        }

        [Theory]
        [InlineData("Aa1!")] // too short
        [InlineData("aaaaaaaa")] // no uppercase, digit, special
        [InlineData("AAAAAAAA")] // no lowercase, digit, special
        [InlineData("12345678")] // no letters, special
        [InlineData("abcdEFGH")] // no digit, special
        [InlineData("abcdEFG1")] // no special
        public void Create_ShouldThrowArgumentException_WhenPasswordIsInvalid(string invalidPassword)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Password.Create(invalidPassword));
        }

        [Theory]
        [InlineData("Aa1!aaaa", true)]
        [InlineData("invalid", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void TryCreate_ShouldReturnExpectedResult(string? input, bool expectedSuccess)
        {
            // Act
            var result = Password.TryCreate(input!, out var password);

            // Assert
            Assert.Equal(expectedSuccess, result);

            if (expectedSuccess)
            {
                Assert.NotNull(password);
                Assert.Equal(input, password!.Value);
            }
            else
            {
                Assert.Null(password);
            }
        }
    }
}
