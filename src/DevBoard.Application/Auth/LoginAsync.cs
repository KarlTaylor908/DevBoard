using DevBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Application.Auth
{
    public class LoginAsync
    {
        private readonly IAuthRepository _authRepository;

        public LoginAsync(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<UserEnt?> ExecuteAsync(string email, string password)
        {
            return await _authRepository.LoginAsync(email, password);
        }
    }
}
