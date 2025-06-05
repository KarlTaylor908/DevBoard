using DevBoard.API.DTOs.Authentication;
using DevBoard.Application.Auth;
using Microsoft.AspNetCore.Mvc;

namespace DevBoard.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                var user = await _authRepository.RegisterAsync(request.Name, request.Email, request.Password);
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
                var user = await _authRepository.LoginAsync(request.Email, request.Password);
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
