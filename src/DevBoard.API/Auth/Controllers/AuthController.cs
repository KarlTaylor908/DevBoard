using DevBoard.API.Auth.Requests;
using DevBoard.API.Auth.Responses;
using DevBoard.Application.Auth;
using DevBoard.Domain.Auth.ValueObjects;
using DevBoard.Infrastructure.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevBoard.API.Auth.Controllers
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
                var emailAddress = EmailAddress.Create(request.Email);
                var password = Password.Create(request.Password);

                var user = await _authService.RegisterAsync(request.Name, emailAddress, password);
                var token = _jwtService.GenerateToken(user);

                if (user == null || user.Email == null)
                    return BadRequest("Invalid Credentials");
                else
                {
                    return Json(new AuthResponse
                    {
                        Email = user.Email.Value,
                        Token = token
                    });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var emailAddressCreated = EmailAddress.TryCreate(request.Email, out var emailAddress);
            var passwordCreated = Password.TryCreate(request.Password, out var password);

            try
            {
                if (emailAddressCreated && passwordCreated && emailAddress != null && password != null)
                {
                    var user = await _authService.LoginAsync(emailAddress, password);

                    if (user == null || user.Email == null)
                        return Unauthorized("Login Failed");

                    var token = _jwtService.GenerateToken(user);

                    return Json(new AuthResponse
                    {
                        Email = user.Email.Value,
                        Token = token
                    });
                }
                else
                {
                    return BadRequest("Login Failed");
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
