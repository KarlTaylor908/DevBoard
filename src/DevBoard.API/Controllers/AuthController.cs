using DevBoard.API.DTOs.Authentication;
using DevBoard.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevBoard.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                var user = await _authService.RegisterAsync(request.Name, request.Email, request.Password);
                return Json(new AuthResponse
                {
                    Email = user.Email,
                    Token = "dummy-token" //Todo - Implement JWT token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var user = await _authService.LoginAsync(request.Email, request.Password);
                if (user == null)
                    return Unauthorized("Invalid Credentials");

                return Json(new AuthResponse
                {
                    Email = user.Email,
                    Token = "dummy-token" //Todo - Implement JWT token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
