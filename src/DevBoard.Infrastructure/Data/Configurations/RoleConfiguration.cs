using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DevBoard.Domain.Auth.Entities;

namespace DevBoard.Infrastructure.Data.Configurations
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
