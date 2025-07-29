using DevBoard.Domain.Shared;
using DevBoard.Domain.User.Entities;

namespace DevBoard.Domain.Tickets.Entities
{
    public class TicketEnt : BaseEnt
    {

        public TicketEnt() { }

        public TicketEnt(string name)
        {
            Name = name;
        }

        public string Name { get; set; } = string.Empty;
        public Guid? AssignedId { get; set; }
        public UserEnt? Assigned { get; set; } 

        public void AssignUser(UserEnt assigned)
        {
            AssignedId = assigned.Id;
            Assigned = assigned;
        }
    }
}
