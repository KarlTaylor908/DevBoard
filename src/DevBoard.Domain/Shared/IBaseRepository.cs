namespace DevBoard.Domain.Shared
{
    public interface IBaseRepository<T> where T : BaseEnt
    {
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(T ent);
        Task<List<T>> Get();
        Task SaveChangesAsync();
        Task<T?> GetById(Guid Id);
    }
}
