using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces
{
    public interface IMusicRepository : IGenericRepository<Music>
    {
        Task<IReadOnlyList<string>> GetGenres();
    }
}
