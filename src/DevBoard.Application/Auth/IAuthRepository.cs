using DevBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Application.Auth
{
    public interface IAuthRepository
    {
        Task<UserEnt> RegisterAsync(string name, string email, string password);
        Task<UserEnt?> LoginAsync(string email, string password);
    }
}
