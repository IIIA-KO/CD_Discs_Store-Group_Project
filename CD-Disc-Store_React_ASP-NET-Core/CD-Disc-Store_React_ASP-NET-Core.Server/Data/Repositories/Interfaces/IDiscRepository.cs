using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces
{
    public interface IDiscRepository : IGenericRepository<Disc>
    {
        Task<IReadOnlyList<Film>> GetFilmsOnDiscAsync(Guid? id);
        Task<IReadOnlyList<Music>> GetMusicOnDiscAsync(Guid? id);
    }
}
