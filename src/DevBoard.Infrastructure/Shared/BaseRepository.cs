using DevBoard.Domain.Shared;
using DevBoard.Infrastructure.Data;
using DevBoard.Infrastructure.Migrations;
using Microsoft.EntityFrameworkCore;

namespace DevBoard.Infrastructure.Shared
{
    public class BaseRepository<T> where T : BaseEnt
    {
        protected readonly AppDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(AppDbContext db)
        {
            _dbContext = db;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task AddAsync(T ent)
        {
            await _dbSet.AddAsync(ent);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbSet.AnyAsync(u => u.Id == id);
        }

        public virtual async Task<List<T>> GetAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }
    }
}
