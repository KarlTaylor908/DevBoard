using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevBoard.Domain.Shared;

namespace DevBoard.Domain.User.Entities
{
    public class UserRoleEnt : BaseEnt
    {
        public Guid UserId { get; set; }
        public UserEnt User { get; set; } = new();
        public Guid RoleId { get; set; }
        public RoleEnt Role { get; set; } = new();
    }
}
