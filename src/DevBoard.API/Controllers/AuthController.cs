using DevBoard.API.DTOs.Authentication;
using DevBoard.Application.Auth;
using DevBoard.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace DevBoard.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly JwtService _jwtService;

        public AuthController(IAuthService authService, JwtService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                var user = await _authService.RegisterAsync(request.Name, request.Email, request.Password);
                var token = _jwtService.GenerateToken(user);

                return Json(new AuthResponse
                {
                    Email = user.Email,
                    Token = token
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

                if (user != null)
                {
                    var token = _jwtService.GenerateToken(user);

                    if (user == null)
                        return Unauthorized("Invalid Credentials");

                    return Json(new AuthResponse
                    {
                        Email = user.Email,
                        Token = token
                    });
                }

                else throw new Exception("Login Failed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
