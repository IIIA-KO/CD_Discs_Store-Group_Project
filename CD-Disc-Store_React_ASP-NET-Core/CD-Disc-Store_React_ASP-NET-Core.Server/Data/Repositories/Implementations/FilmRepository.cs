using Dapper;
using System.Data;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Processors;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Implementations
{
    public class FilmRepository(IDapperContext context, Processor<Film> processor)
        : GenericRepository<Film>(context, processor), IFilmRepository
    {
        public async Task<IReadOnlyList<string>> GetGenres()
        {
            var query = "SELECT Distinct Genre FROM Film";
            using IDbConnection dbConnection = this._context.CreateConnection();
            return (IReadOnlyList<string>)await dbConnection.QueryAsync<string>(query);
        }
    }
}
