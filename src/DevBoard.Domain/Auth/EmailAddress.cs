using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DevBoard.Domain.Auth
{
    public sealed class EmailAddress
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public string Value { get; }

        private EmailAddress(string value) { Value = value; }

        public static EmailAddress Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required", nameof(email));

            if (!EmailRegex.IsMatch(email))
                throw new ArgumentException("Invalid email format", nameof(email));

            return new EmailAddress(email);
        }

        public static bool TryCreate(string email, out EmailAddress? emailAddress)
        {
            emailAddress = null;

            if (!IsValid(email))
                return false;

            emailAddress = new EmailAddress(email);
            return true;
        }

        public static bool IsValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return EmailRegex.IsMatch(email);
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj) =>
            obj is EmailAddress other && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode();
    }
}
