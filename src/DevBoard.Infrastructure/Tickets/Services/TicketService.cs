using DevBoard.Application.Auth;
using DevBoard.Application.Tickets.Interfaces;
using DevBoard.Domain.Tickets;
using DevBoard.Domain.Tickets.Entities;
using DevBoard.Domain.User.Entities;

namespace DevBoard.Infrastructure.Tickets.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;

        public TicketService(ITicketRepository ticketRepository, IUserRepository userRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
        }

        public async Task<TicketEnt?> CreateTicketAsync(string name, Guid? assignedId)
        {

            UserEnt? assigned = null;
            var ticket = new TicketEnt(name);

            if (assignedId is not null)
            {
                assigned = _userRepository.GetById(assignedId.Value).Result;

                if (assigned is null)
                    throw new InvalidDataException("Invalid Assigned Id");

                ticket.AssignUser(assigned);
            }

            await _ticketRepository.AddAsync(ticket);
            await _ticketRepository.SaveChangesAsync();

            return ticket;

        }
    }
}
