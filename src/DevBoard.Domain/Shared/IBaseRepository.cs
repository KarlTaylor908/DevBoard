namespace DevBoard.Domain.Shared
{
    public interface IBaseRepository<T> where T : BaseEnt
    {
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(T ent);
        Task<List<T>> GetAsync();
        Task SaveChangesAsync();
        Task<T?> GetByIdAsync(Guid Id);
    }
}
