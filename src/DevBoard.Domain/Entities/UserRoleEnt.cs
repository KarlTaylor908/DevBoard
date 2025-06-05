using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Domain.Entities
{
    public class UserRoleEnt : BaseEnt
    {
        public Guid UserId { get; set; }
        public UserEnt User { get; set; } = new();
        public Guid RoleId { get; set; }
        public RoleEnt Role { get; set; } = new();
    }
}
