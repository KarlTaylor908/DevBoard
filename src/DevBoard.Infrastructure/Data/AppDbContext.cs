
using DevBoard.Domain.Boards.Entities;
using DevBoard.Domain.User.Entities;
using DevBoard.Domain.Tickets.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DevBoard.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserEnt> Users { get; set; }
        public DbSet<RoleEnt> Roles { get; set; }
        public DbSet<UserRoleEnt> UserRoles { get; set; }
        public DbSet<TicketEnt> Tickets { get; set; }
        public DbSet<BoardEnt> Boards { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
