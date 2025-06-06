using DevBoard.Domain.Auth.Entities;
using DevBoard.Domain.Shared;

namespace DevBoard.Application.Auth
{
    public interface IUserRepository : IBaseRepository<UserEnt>
    {
        Task<UserEnt?> GetUserByEmailAsync(string email);
        Task<bool> UserEmailExistsAsync(string email);
    }
}
