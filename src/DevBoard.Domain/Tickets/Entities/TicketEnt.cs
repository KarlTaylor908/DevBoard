using DevBoard.Domain.Auth.Entities;
using DevBoard.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Domain.Tickets.Entities
{
    public class TicketEnt : BaseEnt
    {
        public string Name { get; set; }
        public Guid AssignedId { get; set; }
        public UserEnt Assigned { get; set; }
    }
}
