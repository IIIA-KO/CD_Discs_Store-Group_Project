using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using Microsoft.Data.SqlClient;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid? id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        bool IsEntityChanged(T currentEntity, T entity);
        Task<int> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<IReadOnlyList<T>> GetProcessedAsync(string? searchText, SortOrder sortOrder, string? sortField, int skip, int pageSize);
        Task<int> CountProcessedDataAsync(string? searchText);
    }
}
