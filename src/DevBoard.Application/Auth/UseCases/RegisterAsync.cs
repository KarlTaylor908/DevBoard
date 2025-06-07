using DevBoard.Domain.Auth.Entities;
using DevBoard.Domain.Auth.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Application.Auth.UseCases
{
    public class RegisterAsync
    {
        private readonly IAuthService _authService;

        public RegisterAsync(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<UserEnt?> ExecuteAsync(string name, EmailAddress email, Password password)
        {
            return await _authService.RegisterAsync(name, email, password);
        }
    }
}
