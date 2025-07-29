using DevBoard.Domain.Tickets;
using DevBoard.Domain.Tickets.Entities;
using DevBoard.Infrastructure.Data;
using DevBoard.Infrastructure.Shared;

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
