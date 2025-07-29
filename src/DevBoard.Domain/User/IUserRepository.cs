using DevBoard.Domain.User.Entities;
using DevBoard.Domain.User.ValueObjects;
using DevBoard.Domain.Shared;

namespace DevBoard.Application.Auth
{
    public interface IUserRepository : IBaseRepository<UserEnt>
    {
        Task<UserEnt?> GetUserByEmailAsync(EmailAddress email);
        Task<bool> UserEmailExistsAsync(EmailAddress email);
    }
}
