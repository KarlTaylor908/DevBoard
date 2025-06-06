using DevBoard.API.DTOs.Authentication;
using DevBoard.Application.Auth;
using DevBoard.Domain.Auth;
using DevBoard.Infrastructure.Auth.Services;
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
                if (!EmailAddress.TryCreate(request.Email, out var emailAddress))
                    return BadRequest("Invalid email format.");

                if (emailAddress is null)
                    return BadRequest("Email is empty");

                var user = await _authService.RegisterAsync(request.Name, emailAddress, request.Password);
                var token = _jwtService.GenerateToken(user);

                if (user.Email is null)
                    return BadRequest("Email is empty");

                return Json(new AuthResponse
                {
                    Email = user.Email.Value,
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
                if (!EmailAddress.TryCreate(request.Email, out var emailAddress))
                    return BadRequest("Invalid email format.");

                if (emailAddress is null)
                    return BadRequest("Email is empty");

                var user = await _authService.LoginAsync(emailAddress, request.Password);

                if (user != null)
                {
                    var token = _jwtService.GenerateToken(user);

                    if (user == null)
                        return Unauthorized("Invalid Credentials");

                    if (user.Email is null)
                        return BadRequest("Email is empty");

                    return Json(new AuthResponse
                    {
                        Email = user.Email.Value,
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
