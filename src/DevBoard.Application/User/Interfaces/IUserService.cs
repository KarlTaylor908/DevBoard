using DevBoard.Domain.User.Entities;

namespace DevBoard.Application.User.Interfaces
{
    public interface IUserService
    {
        Task<List<UserEnt>> GetUsers();
    }
}
