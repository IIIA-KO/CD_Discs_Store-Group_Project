using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Processors;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Implementations
{
    public class MusicRepository(IDapperContext context, ProcessableViewModelProcessor<Music> processor)
        : GenericRepository<Music>(context, processor), IMusicRepository
    {
    }
}
