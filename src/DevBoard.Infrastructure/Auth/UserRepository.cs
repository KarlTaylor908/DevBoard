using Microsoft.EntityFrameworkCore;
using DevBoard.Application.Auth;
using DevBoard.Infrastructure.Data;
using DevBoard.Domain.User.Entities;
using DevBoard.Infrastructure.Shared;
using DevBoard.Domain.User.ValueObjects;

namespace DevBoard.infrastructure.Auth
{
    public class UserRepository : BaseRepository<UserEnt>, IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<UserEnt?> GetUserByEmailAsync(EmailAddress email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> UserEmailExistsAsync(EmailAddress email)
        {
            return await _db.Users.AnyAsync(u => u.Email == email);
        }
    }
}
