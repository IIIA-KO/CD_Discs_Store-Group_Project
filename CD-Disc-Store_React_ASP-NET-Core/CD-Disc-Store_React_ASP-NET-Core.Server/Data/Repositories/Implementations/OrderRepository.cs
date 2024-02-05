using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Processors;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Implementations
{
    public class OrderRepository(IDapperContext context, ProcessableViewModelProcessor<Order> processor)
        : GenericRepository<Order>(context, processor), IOrderRepository
    {
    }
}
