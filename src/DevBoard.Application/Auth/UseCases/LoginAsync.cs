using DevBoard.Domain.User.Entities;
using DevBoard.Domain.User.ValueObjects;

namespace DevBoard.Application.Auth.UseCases
{
    public class LoginAsync
    {
        private readonly IAuthService _authService;

        public LoginAsync(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<UserEnt?> ExecuteAsync(EmailAddress email, Password password)
        {
            return await _authService.LoginAsync(email, password);
        }
    }
}
