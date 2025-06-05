using DevBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DevBoard.API.Infrastructure.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<RoleEnt>
    {
        public void Configure(EntityTypeBuilder<RoleEnt> builder)
        {
            builder.ToTable("Role");
            builder.HasKey(u => u.Id);
        }
    }
}
