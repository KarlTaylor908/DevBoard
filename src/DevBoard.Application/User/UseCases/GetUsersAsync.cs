using DevBoard.Application.User.Interfaces;
using DevBoard.Domain.User.Entities;

namespace DevBoard.Application.User.UseCases
{
    public class GetUsersAsync
    {
        private readonly IUserService _userService;

        public GetUsersAsync(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<List<UserEnt>> ExecuteAsync()
        {
            return await _userService.GetUsers();
        }
    }
}
