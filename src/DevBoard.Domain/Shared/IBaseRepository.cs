using DevBoard.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Domain.Shared
{
    public interface IBaseRepository<T> where T : BaseEnt
    {
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(T ent);
        Task SaveChangesAsync();
    }
}
