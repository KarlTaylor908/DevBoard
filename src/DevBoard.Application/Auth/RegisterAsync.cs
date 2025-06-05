using DevBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Application.Auth
{
    public class RegisterAsync
    {
        private readonly IAuthRepository _authRepo;

        public RegisterAsync(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        public async Task<UserEnt?> ExecuteAsync(string name, string email, string password)
        {
            return await _authRepo.RegisterAsync(name, email, password);
        }
    }
}
