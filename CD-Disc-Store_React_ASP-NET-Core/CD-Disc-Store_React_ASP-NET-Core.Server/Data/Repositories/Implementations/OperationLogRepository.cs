using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exceptions;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using static Dapper.SqlMapper;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Implementations
{
    public class OperationLogRepository : IOperationLogRepository
    {
        private readonly IDapperContext _context;
        private readonly IOrderRepository _orderRepository;

        private const string OPERATION_LOG_NOT_FOUND_BY_ID_ERROR = "The operation with specified Id was not found";
        private const string ORDER_DOES_NOT_EXIST = "The Order with specified Id does not exist. Cannot Add Operation Log";
        private const string OPERATION_TYPE_DOES_NOT_EXIST = "The Operation Type with specified Id does not exist. Cannot Add Operation Log";

        public OperationLogRepository(IDapperContext context, IOrderRepository orderRepository)
        {
            this._context = context;
            this._orderRepository = orderRepository;
        }
        public async Task<OperationLog> GetByIdAsync(Guid? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id), OPERATION_LOG_NOT_FOUND_BY_ID_ERROR);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            var operationLog = await dbConnection.QueryFirstOrDefaultAsync<OperationLog>("SELECT * FROM OperationLog WHERE Id = @Id", new { Id = id });
            return operationLog ?? throw new NotFoundException(OPERATION_LOG_NOT_FOUND_BY_ID_ERROR);
        }

        public async Task<IReadOnlyList<OperationLog>> GetAllAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            var operationLogs = await dbConnection.QueryAsync<OperationLog>("SELECT * FROM OperationLog");
            return (IReadOnlyList<OperationLog>)operationLogs ?? new List<OperationLog>();
        }

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

        public async Task<int> AddAsync(OperationLog entity)
        {
            if (!await this._orderRepository.ExistsAsync(entity.IdOrder))
            {
                throw new InvalidOperationException(ORDER_DOES_NOT_EXIST);
            }

            if (!await OperationTypeExistsAsync(entity.OperationType))
            {
                throw new InvalidOperationException(OPERATION_TYPE_DOES_NOT_EXIST);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            dbConnection.Open();

            using IDbTransaction transaction = dbConnection.BeginTransaction();

            try
            {
                var result = await dbConnection.ExecuteAsync(
                    "INSERT INTO OperationLog (Id, OperationType, OperationDateTimeStart, OperationDateTimeEnd, IdOrder)" +
                    "VALUES (@Id, @OperationType, @OperationDateTimeStart, @OperationDateTimeEnd, @IdOrder)",
                    entity, transaction);

                transaction.Commit();
                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        private async Task<bool> OperationTypeExistsAsync(Guid id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteScalarAsync<bool>("SELECT COUNT(1) FROM OperationType WHERE Id = @Id", new { Id = id });
        }

        public async Task<int> UpdateAsync(OperationLog entity)
        {
            OperationLog currentOperationLog;
            try
            {
                currentOperationLog = await this.GetByIdAsync(entity.Id);
            }
            catch (NullReferenceException)
            {
                throw;
            }

            if (currentOperationLog != null && !IsEntityChanged(currentOperationLog, entity))
            {
                return 0;
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteAsync("UPDATE OperationLog SET OperationType = @OperationType, OperationDateTimeStart = @OperationDateTimeStart, OperationDateTimeEnd = @OperationDateTimeEnd, IdOrder = @IdOrder", entity);
        }

        public bool IsEntityChanged(OperationLog currentEntity, OperationLog entity)
        {
            return currentEntity.OperationDateTimeStart != entity.OperationDateTimeStart
                || currentEntity.OperationDateTimeEnd != entity.OperationDateTimeEnd
                || currentEntity.OperationType != entity.OperationType
                || currentEntity.IdOrder != entity.IdOrder;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            if (!await ExistsAsync(id))
            {
                return 0;
            }

            return await dbConnection.ExecuteAsync($"DELETE FROM OperationLog WHERE Id = @Id", new { Id = id });
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteScalarAsync<bool>("SELECT COUNT(1) FROM OperationLog WHERE Id = @Id", new { Id = id });
        }

        public Task<IReadOnlyList<OperationLog>> GetProcessedAsync(string? searchText, SortOrder sortOrder, string? sortField, int skip, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountProcessedDataAsync(string? searchText)
        {
            throw new NotImplementedException();
        }
    }
}