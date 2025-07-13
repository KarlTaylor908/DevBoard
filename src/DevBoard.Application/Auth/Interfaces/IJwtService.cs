using DevBoard.Domain.User.Entities;

namespace DevBoard.Application.Auth.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(UserEnt user);
    }
}
