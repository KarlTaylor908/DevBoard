using DevBoard.Domain.Auth;
using DevBoard.Domain.Auth.Entities;

namespace DevBoard.Application.Auth
{
    public interface IAuthService
    {
        Task<UserEnt?> LoginAsync(EmailAddress email, string password);
        Task<UserEnt> RegisterAsync(string name, EmailAddress email, string password);
    }
}
