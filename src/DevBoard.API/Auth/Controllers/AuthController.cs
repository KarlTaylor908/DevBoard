using DevBoard.API.Auth.Requests;
using DevBoard.API.Auth.Responses;
using DevBoard.Application.Auth;
using DevBoard.Application.Auth.Interfaces;
using DevBoard.Domain.User.ValueObjects;
using DevBoard.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DevBoard.API.Auth.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly JwtSettings _jwtSettings;

        public AuthController(IAuthService authService, IJwtService jwtService, IOptions<JwtSettings> jwtOptions)
        {
            _authService = authService;
            _jwtService = jwtService;
            _jwtSettings = jwtOptions.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                var emailAddress = EmailAddress.Create(request.Email);
                var password = Password.Create(request.Password);

                var user = await _authService.RegisterAsync(request.Name, emailAddress, password);

                if (user == null || user.Email == null)
                    return BadRequest("Invalid Credentials");
                else
                {
                    var token = _jwtService.GenerateToken(user);

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

                    // Set token as HttpOnly cookie
                    HttpContext.Response.Cookies.Append("jwt_token", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true, // important in prod
                        SameSite = SameSiteMode.Strict, // adjust for your app
                        Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes)
                    });

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
