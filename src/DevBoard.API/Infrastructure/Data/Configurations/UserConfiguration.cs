using DevBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevBoard.API.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEnt>
    {
        public void Configure(EntityTypeBuilder<UserEnt> builder)
        {
            builder.ToTable("User");
            builder.HasKey(u => u.Id);
        }
    }
}
