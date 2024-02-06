using Dapper;
using System.Data;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Processors;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Implementations
{
    public class DiscRepository(IDapperContext context, ProcessableViewModelProcessor<Disc> processor)
        : GenericRepository<Disc>(context, processor), IDiscRepository
    {
        public async Task<IReadOnlyList<Film>> GetFilmsOnDiscAsync(Guid? id)
        {
            if (id is null)
            {
                throw new NullReferenceException(GetNotFoundErrorMessage());
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            var query = @"
                SELECT
                    Film.*
                FROM
                    Disc
                JOIN DiscFilm ON Disc.Id = DiscFilm.IdDisc
                JOIN Film ON DiscFilm.IdFilm = Film.Id
                WHERE Disc.Id = @DiscId;";

            return (IReadOnlyList<Film>)await dbConnection.QueryAsync<Film>(query, new { DiscId = id });
        }

        public async Task<IReadOnlyList<Music>> GetMusicOnDiscAsync(Guid? id)
        {
            if (id is null)
            {
                throw new NullReferenceException(GetNotFoundErrorMessage());
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            var query = @"
                SELECT
                    Music.*
                FROM
                    Disc
                JOIN DiscMusic ON Disc.Id = DiscMusic.IdDisc
                JOIN Music ON DiscMusic.IdMusic = Music.Id
                WHERE Disc.Id = @DiscId;";

            return (IReadOnlyList<Music>)await dbConnection.QueryAsync<Music>(query, new { DiscId = id });
        }
    }
}
