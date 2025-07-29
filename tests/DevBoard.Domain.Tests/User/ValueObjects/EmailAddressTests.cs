using DevBoard.Domain.User.ValueObjects;

namespace DevBoard.Domain.Tests.User.ValueObjects
{
    public class EmailAddressTests
    {
        [Theory]
        [InlineData("test@example.com")]
        [InlineData("user.name@domain.co.uk")]
        [InlineData("user+alias@domain.io")]
        public void Create_ShouldReturnEmailAddress_WhenEmailIsValid(string validEmail)
        {
            // Act
            var emailAddress = EmailAddress.Create(validEmail);

            // Assert
            Assert.NotNull(emailAddress);
            Assert.Equal(validEmail, emailAddress.Value);
            Assert.Equal(validEmail.ToLowerInvariant(), emailAddress.ToString());
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Create_ShouldThrowArgumentNullException_WhenEmailIsNullOrWhitespace(string? invalidEmail)
        {
            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => EmailAddress.Create(invalidEmail!));
        }

        [Theory]
        [InlineData("plainaddress")]
        [InlineData("@missingusername.com")]
        [InlineData("username@.com")]
        [InlineData("username@com")]
        [InlineData("username@domain..com")]
        public void Create_ShouldThrowArgumentException_WhenEmailIsInvalidFormat(string invalidEmail)
        {
            // Act && Assert
            Assert.Throws<ArgumentException>(() => EmailAddress.Create(invalidEmail));
        }

        [Theory]
        [InlineData("valid@example.com", true)]
        [InlineData("invalid-email", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void TryCreate_ShouldReturnExpectedResult(string? input, bool expectedSuccess)
        {
            // Act
            var result = EmailAddress.TryCreate(input!, out var emailAddress);

            // Assert
            Assert.Equal(expectedSuccess, result);

            if (expectedSuccess)
            {
                Assert.NotNull(emailAddress);
                Assert.Equal(input, emailAddress.Value);
            }
            else
            {
                Assert.Null(emailAddress);
            }
        }
    }
}
