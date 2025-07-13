using DevBoard.Application.User.Interfaces;
using DevBoard.Domain.User.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevBoard.API.User.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]

    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                List<UserEnt> users = await _userService.GetUsers();
                if (users.Count <= 0)
                    return BadRequest("No users were found.");

                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
