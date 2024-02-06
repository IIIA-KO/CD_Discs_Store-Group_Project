using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Processors;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Implementations
{
    public class FilmRepository(IDapperContext context, Processor<Film> processor)
        : GenericRepository<Film>(context, processor), IFilmRepository
    {
    }
}
