using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using Microsoft.Data.SqlClient;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces
{
    public interface IMusicRepository : IGenericRepository<Music>
    {
        Task<IReadOnlyList<Music>> GetProcessedAsync(string? searchText, SortOrder sortOrder, string? sortField, int skip, int pageSize);

        Task<int> CountProcessedDataAsync(string? searchText);
    }
}
