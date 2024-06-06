using Dapper;
using System.Data;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Processors;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Implementations
{
    public class OrderItemRepository(IDapperContext context, Processor<OrderItem> processor)
        : GenericRepository<OrderItem>(context, processor), IOrderItemRepository
    {
        public async Task<IReadOnlyList<OrderItem>> GetByOrderIdAsync(Guid orderId)
        {
            IDbConnection dbConnection = this._context.CreateConnection();
            var items = await dbConnection.QueryAsync<OrderItem>("SELECT * FROM OrderItem WHERE IdOrder = @OrderId", new { OrderId = orderId });

            return (IReadOnlyList<OrderItem>)items ?? new List<OrderItem>();
        }
    }
}
