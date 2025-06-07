using DevBoard.Domain.Auth.Entities;
using DevBoard.Domain.Auth.ValueObjects;
using DevBoard.Domain.Shared;

namespace DevBoard.Application.Auth
{
    public interface IUserRepository : IBaseRepository<UserEnt>
    {
        Task<UserEnt?> GetUserByEmailAsync(EmailAddress email);
        Task<bool> UserEmailExistsAsync(EmailAddress email);
    }
}
