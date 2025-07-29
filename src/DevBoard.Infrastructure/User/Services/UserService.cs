using DevBoard.Application.Auth;
using DevBoard.Application.User.Interfaces;
using DevBoard.Domain.User.Entities;

namespace DevBoard.Infrastructure.User.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserEnt>> GetUsers()
        {
            return await _userRepository.GetAsync();
        }
    }
}
