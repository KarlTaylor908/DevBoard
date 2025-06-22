using DevBoard.Domain.Tickets.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Infrastructure.Data.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<TicketEnt>
    {
        public void Configure(EntityTypeBuilder<TicketEnt> builder)
        {
            builder.ToTable("Ticket");
            builder.HasKey(t => t.Id);

            builder
                .HasOne(u => u.Assigned)
                .WithMany()
                .HasForeignKey(u => u.AssignedId);
        }
    }
}
