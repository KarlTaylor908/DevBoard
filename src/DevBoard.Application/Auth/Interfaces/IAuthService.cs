using DevBoard.Domain.User.Entities;
using DevBoard.Domain.User.ValueObjects;

namespace DevBoard.Application.Auth
{
    public interface IAuthService
    {
        Task<UserEnt?> LoginAsync(EmailAddress email, Password password);
        Task<UserEnt?> RegisterAsync(string name, EmailAddress email, Password password);
    }
}
