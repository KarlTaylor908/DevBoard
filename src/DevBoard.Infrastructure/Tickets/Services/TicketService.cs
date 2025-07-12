using DevBoard.Application.Tickets.Interfaces;
using DevBoard.Domain.Tickets;
using DevBoard.Domain.Tickets.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Infrastructure.Tickets.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<TicketEnt?> CreateTicketAsync(string name)
        {
            var ticket = new TicketEnt(name);

            await _ticketRepository.AddAsync(ticket);
            await _ticketRepository.SaveChangesAsync();

            return ticket;
        }
    }
}
