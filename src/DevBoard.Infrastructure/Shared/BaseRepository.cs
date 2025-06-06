using DevBoard.Domain.Auth.Entities;
using DevBoard.Domain.Shared;
using DevBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
