using DevBoard.Domain.Auth.Entities;
using DevBoard.Domain.Auth.ValueObjects;
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

        public async Task<UserEnt?> ExecuteAsync(EmailAddress email, Password password)
        {
            return await _authService.LoginAsync(email, password);
        }
    }
}
