namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid? id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<IReadOnlyList<T>> GetProcessedAsync(ProcessableViewModel<T> processable);
    }
}
