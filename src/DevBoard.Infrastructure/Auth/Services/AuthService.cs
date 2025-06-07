using DevBoard.Application.Auth;
using DevBoard.Domain.Auth.Entities;
using DevBoard.Domain.Auth.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DevBoard.Infrastructure.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<UserEnt> _hasher = new();
        private readonly IOptions<LockoutOptions> _lockoutOptions;

        public AuthService(IUserRepository userRepository, IOptions<LockoutOptions> lockoutOptions)
        {
            _userRepository = userRepository;
            _lockoutOptions = lockoutOptions;
        }

        public async Task<UserEnt?> LoginAsync(EmailAddress email, Password password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                return null;

            if (user.IsLockedOut())
                throw new Exception($"Account locked until {user.LockoutUntil:HH:mm:ss}");

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password.Value);

            if (result == PasswordVerificationResult.Success)
            {
                user.ResetFailedAttempts();
                await _userRepository.SaveChangesAsync();
                return user;
            }

            user.RegisterFailedAttempt(
                _lockoutOptions.Value.MaxFailedAccessAttempts,
                _lockoutOptions.Value.DefaultLockoutTimeSpan
            );

            await _userRepository.SaveChangesAsync();

            return null;
        }

        public async Task<UserEnt> RegisterAsync(string name, EmailAddress email, Password password)
        {
            if (await _userRepository.UserEmailExistsAsync(email))
                throw new Exception("User already exists");

            var user = new UserEnt(name, email);
            user.SetPasswordHash(_hasher.HashPassword(user, password.Value));

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return user;
        }
    }
}
