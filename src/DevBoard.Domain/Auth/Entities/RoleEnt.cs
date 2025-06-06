using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevBoard.Domain.Shared;

namespace DevBoard.Domain.Auth.Entities
{
    public class RoleEnt : BaseEnt
    {
        public string Name { get; set; } = string.Empty;
    }
}
