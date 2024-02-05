using Dapper;
using System.Data;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Processors;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Implementations
{
    public class OperationLogRepository(IDapperContext context, ProcessableViewModelProcessor<OperationLog> processor)
        : GenericRepository<OperationLog>(context, processor), IOperationLogRepository
    {
        public async Task<IReadOnlyList<OperationLog>> GetByClientIdAsync(Guid? id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            var operationLogs = await dbConnection.QueryAsync<OperationLog>("SELECT ol.* FROM OperationLog AS ol INNER JOIN [Order] as o ON ol.IdOrder = o.Id WHERE o.IdClient = @IdClient", new { IdClient = id });
            return (IReadOnlyList<OperationLog>)operationLogs ?? new List<OperationLog>();
        }

        public async Task<IReadOnlyList<OperationLog>> GetByDiscIdAsync(Guid? id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            var operationLogs = await dbConnection.QueryAsync<OperationLog>(
               $@"SELECT ol.*
               FROM OperationLog AS ol
               JOIN [Order] ON ol.IdOrder = [Order].Id
               JOIN OperationType ON ol.OperationType = OperationType.Id
               JOIN OrderItem ON [Order].Id = OrderItem.IdOrder
               JOIN Disc ON OrderItem.IdDisc = Disc.Id
               WHERE Disc.Id = @IdDisc",
                new { IdDisc = id });
            return (IReadOnlyList<OperationLog>)operationLogs ?? new List<OperationLog>();
        }
    }
}
