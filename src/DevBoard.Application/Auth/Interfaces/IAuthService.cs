using DevBoard.Domain.Auth.Entities;

namespace DevBoard.Application.Auth
{
    public interface IAuthService
    {
        Task<UserEnt?> LoginAsync(string email, string password);
        Task<UserEnt> RegisterAsync(string name, string email, string password);
    }
}
