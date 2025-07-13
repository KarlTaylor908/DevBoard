using DevBoard.Domain.Auth.Entities;
using DevBoard.Domain.Tickets.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Application.Tickets.Interfaces
{
    public interface ITicketService
    {
        Task<TicketEnt?> CreateTicketAsync(string name, Guid? assignedId);
    }
}
