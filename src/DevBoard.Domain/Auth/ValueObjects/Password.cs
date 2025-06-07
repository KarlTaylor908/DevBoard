using DevBoard.Domain.Shared;
using System.Text.RegularExpressions;

namespace DevBoard.Domain.Auth.ValueObjects
{
    public sealed class Password : ValueObject
    {

        private const int MinLength = 8;

        private static readonly Regex ComplexityRegex = new Regex(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{" + MinLength + ",}$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public string Value { get; }

        private Password(string value) { Value = value; }

        public static Password Create(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Password is required", nameof(password));

            if (password.Length < MinLength)
                throw new ArgumentException($"Password must be at least {MinLength} characters long.", nameof(password));

            if (!ComplexityRegex.IsMatch(password))
                throw new ArgumentException("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.", nameof(password));

            return new Password(password);
        }

        public static bool TryCreate(string value, out Password? password)
        {
            password = null;
            try
            {
                password = Create(value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value.ToLowerInvariant();
        }
    }
}
