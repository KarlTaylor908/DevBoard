using DevBoard.Domain.Auth.Entities;
using DevBoard.Domain.Boards.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Infrastructure.Data.Configurations
{
    public class BoardConfiguration : IEntityTypeConfiguration<BoardEnt>
    {
        public void Configure(EntityTypeBuilder<BoardEnt> builder)
        {
            builder.ToTable("Board");
            builder.HasKey(t => t.Id);

            builder
                .HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(u => u.UserId);
        }
    }
}
