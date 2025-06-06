using DevBoard.Domain.Auth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Application.Auth.UseCases
{
    public class LoginAsync
    {
        private readonly IAuthService _authService;

        public LoginAsync(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<UserEnt?> ExecuteAsync(string email, string password)
        {
            return await _authService.LoginAsync(email, password);
        }
    }
}
