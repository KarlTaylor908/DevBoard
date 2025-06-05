using DevBoard.API.Infrastructure.Data;
using DevBoard.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DevBoard.API.Services
{
    public class AuthService
    {
        private readonly AppDbContext _db;
        private readonly PasswordHasher<UserEnt> _hasher = new();
        private readonly IOptions<LockoutOptions>  _lockoutOptions;

        public AuthService(AppDbContext db, IOptions<LockoutOptions> lockoutOptions)
        {
            _db = db;
            _lockoutOptions = lockoutOptions;
        }

        public async Task<UserEnt> RegisterAsync(string name, string email, string password)
        {
            if (await _db.Users.AnyAsync(u => u.Email == email))
                throw new Exception("User already exists"); // Todo - Standardise exceptions

            var user = new UserEnt (name, email, DateTime.UtcNow);
            user.SetPasswordHash(_hasher.HashPassword(user, password));

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<UserEnt?> LoginAsync(string email, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return null;

            if (user.IsLockedOut())
                throw new Exception($"Account locked until {user.LockoutUntil:HH:mm:ss}"); // Todo - Standardise exceptions

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (result == PasswordVerificationResult.Success)
            {
                user.ResetFailedAttempts();
                await _db.SaveChangesAsync();
                return user;
            }

            user.RegisterFailedAttempt(_lockoutOptions.Value.MaxFailedAccessAttempts, _lockoutOptions.Value.DefaultLockoutTimeSpan);
            await _db.SaveChangesAsync();

            return null;
        }
    }
}
