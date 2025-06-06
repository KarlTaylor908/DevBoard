using Microsoft.EntityFrameworkCore;
using DevBoard.Application.Auth;
using DevBoard.Infrastructure.Data;
using DevBoard.Domain.Auth.Entities;
using DevBoard.Infrastructure.Shared;
using DevBoard.Domain.Auth;

namespace DevBoard.Infratructure.Auth
{
    public class AuthRepository : BaseRepository<UserEnt>, IUserRepository
    {
        private readonly AppDbContext _db;

        public AuthRepository(AppDbContext db) : base(db)
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
