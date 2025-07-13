using DevBoard.Domain.User.ValueObjects;
using DevBoard.Domain.Shared;

namespace DevBoard.Domain.User.Entities
{
    public class UserEnt : BaseEnt
    {
        public string Name { get; private set; } = string.Empty;
        public EmailAddress? Email { get; private set; }
        public string PasswordHash { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        // Lockout fields
        public int FailedLoginAttempts { get; private set; } = 0;
        public DateTime? LockoutUntil { get; private set; }

        public UserEnt() { }

        public UserEnt(string name, EmailAddress? email)
        {
            Name = name;
            Email = email;
        }

        public bool IsLockedOut() =>
            LockoutUntil.HasValue && LockoutUntil.Value > DateTime.UtcNow;

        public void RegisterFailedAttempt(int maxAttempts, TimeSpan lockoutTimeSpan)
        {
            // Todo - Send email to user notifying and perhaps allowing them to reset failed attempts
            FailedLoginAttempts++;
            if (FailedLoginAttempts >= maxAttempts)
                LockoutUntil = DateTime.UtcNow.Add(lockoutTimeSpan);
        }

        public void ResetFailedAttempts()
        {
            FailedLoginAttempts = 0;
            LockoutUntil = null;
        }

        public void SetPasswordHash(string hash) => PasswordHash = hash;
    }
}
