using DevBoard.Domain.Auth.Entities;
using DevBoard.Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Domain.Tickets.Entities
{
    public class TicketEnt : BaseEnt
    {
        public TicketEnt(string name)
        {
            Name = name;
        }

        public string Name { get; set; } = string.Empty;
        public Guid? AssignedId { get; set; }
        public UserEnt? Assigned { get; set; } 
    }
}
