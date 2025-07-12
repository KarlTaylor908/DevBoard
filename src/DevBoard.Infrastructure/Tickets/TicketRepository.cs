using DevBoard.Domain.Tickets;
using DevBoard.Domain.Tickets.Entities;
using DevBoard.Infrastructure.Data;
using DevBoard.Infrastructure.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Infrastructure.Tickets
{
    public class TicketRepository : BaseRepository<TicketEnt>, ITicketRepository
    {
        private readonly AppDbContext _db;

        public TicketRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

    }
}
