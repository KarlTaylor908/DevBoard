using DevBoard.Domain.Auth.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DevBoard.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserEnt> Users { get; set; }
        public DbSet<RoleEnt> Roles { get; set; }
        public DbSet<UserRoleEnt> UserRoles { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
