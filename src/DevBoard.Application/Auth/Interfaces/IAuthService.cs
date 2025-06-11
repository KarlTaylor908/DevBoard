using DevBoard.Domain.Auth.Entities;
using DevBoard.Domain.Auth.ValueObjects;

namespace DevBoard.Application.Auth
{
    public interface IAuthService
    {
        Task<UserEnt?> LoginAsync(EmailAddress email, Password password);
        Task<UserEnt?> RegisterAsync(string name, EmailAddress email, Password password);
    }
}
