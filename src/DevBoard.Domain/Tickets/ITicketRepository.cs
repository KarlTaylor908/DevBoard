using DevBoard.Domain.Shared;
using DevBoard.Domain.Tickets.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Domain.Tickets
{
    public interface ITicketRepository : IBaseRepository<TicketEnt>
    {

    }
}
