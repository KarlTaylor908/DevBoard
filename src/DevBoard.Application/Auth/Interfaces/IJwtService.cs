using DevBoard.Domain.User.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Application.Auth.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(UserEnt user);
    }
}
