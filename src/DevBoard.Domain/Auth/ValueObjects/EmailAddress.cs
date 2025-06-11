using DevBoard.Domain.Shared;
using System.Text.RegularExpressions;

namespace DevBoard.Domain.Auth.ValueObjects
{
    public sealed class EmailAddress : ValueObject
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^[^@\s]+@(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        public string Value { get; }

        private EmailAddress(string value) { Value = value; }

        public static EmailAddress Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("Email is required", nameof(email));

            if (!EmailRegex.IsMatch(email))
                throw new ArgumentException("Invalid email format", nameof(email));

            return new EmailAddress(email);
        }

        public static bool TryCreate(string value, out EmailAddress? emailAddress)
        {
            emailAddress = null;

            try 
            { 
                emailAddress = Create(value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override string ToString() => Value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value.ToLowerInvariant();
        }
    }
}
