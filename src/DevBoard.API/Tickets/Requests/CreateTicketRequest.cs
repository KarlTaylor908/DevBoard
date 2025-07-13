using DevBoard.Domain.Auth.Entities;

namespace DevBoard.API.Tickets.Requests
{
    public class CreateTicketRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid? AssignedId { get; set; }
    }
}
