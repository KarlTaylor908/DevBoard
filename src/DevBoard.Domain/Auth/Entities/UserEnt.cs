using DevBoard.Domain.Shared;

namespace DevBoard.Domain.Auth.Entities
{
    public class UserEnt : BaseEnt
    {
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }

        // Lockout fields
        public int FailedLoginAttempts { get; private set; } = 0;
        public DateTime? LockoutUntil { get; private set; }

        public UserEnt() { }

        public UserEnt(string name, string email, DateTime createdAt)
        {
            Name = name;
            Email = email;
            CreatedAt = createdAt;
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
