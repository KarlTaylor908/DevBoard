using DevBoard.Domain.Auth;
using DevBoard.Domain.Auth.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevBoard.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEnt>
    {
        public void Configure(EntityTypeBuilder<UserEnt> builder)
        {
            builder.ToTable("User");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Email)
                .HasConversion(
                    email => email != null ? email.Value : null,
                    value => value != null ? EmailAddress.Create(value) : null)
                .IsRequired();
        }
    }
}
