using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exceptions;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDapperContext _context;
        private readonly IClientRepository _clientRepository;

        private const string ORDER_NOT_FOUND_BY_ID = "The order with specified Id was not found.";
        private const string CLIENT_DOES_NOT_EXIST = "The Client with specified Id does not exist. Cannot Add Order";

        public OrderRepository(IDapperContext context, IClientRepository clientRepository)
        {
            this._context = context;
            this._clientRepository = clientRepository;
        }

        public async Task<Order> GetByIdAsync(Guid? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id), ORDER_NOT_FOUND_BY_ID);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            var order = await dbConnection.QueryFirstOrDefaultAsync<Order>("SELECT * FROM [Order] WHERE Id = @Id", new { Id = id });
            return order ?? throw new NotFoundException(ORDER_NOT_FOUND_BY_ID);
        }

        public async Task<IReadOnlyList<Order>> GetAllAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            var orders = await dbConnection.QueryAsync<Order>("SELECT * FROM [Order]");
            return (IReadOnlyList<Order>)orders ?? new List<Order>();
        }

        public async Task<int> AddAsync(Order entity)
        {
            if (!await this._clientRepository.ExistsAsync(entity.IdClient))
            {
                throw new InvalidOperationException(CLIENT_DOES_NOT_EXIST);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            dbConnection.Open();

            using IDbTransaction transaction = dbConnection.BeginTransaction();

            try
            {
                var result = await dbConnection.ExecuteAsync(
                    "INSERT INTO [Order] (Id, OrderDateTime, IdClient) VALUES(@Id, @OrderDateTime, @IdClient)",
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

        public async Task<int> UpdateAsync(Order entity)
        {
            Order currentOrder;

            try
            {
                currentOrder = await this.GetByIdAsync(entity.Id);
            }
            catch (Exception ex)
                when (ex is ArgumentNullException
                    || ex is NullReferenceException
                    || ex is NotFoundException)
            {
                throw;
            }

            if (currentOrder != null && !IsEntityChanged(currentOrder, entity))
            {
                return 0;
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteAsync("UPDATE [Order] SET OrderDateTime = @OrderDateTime, IdClient = @IdClient WHERE Id = @Id", entity);
        }
        public bool IsEntityChanged(Order currentEntity, Order entity)
        {
            return currentEntity.OrderDateTime != entity.OrderDateTime
                || currentEntity.IdClient != entity.IdClient;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            if (!await this.ExistsAsync(id))
            {
                return 0;
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteAsync("DELETE FROM [Order] WHERE Id = @Id", new { Id = id });
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteScalarAsync<bool>("SELECT COUNT(1) FROM [Order] WHERE Id = @Id", new { Id = id });
        }

        public async Task<IReadOnlyList<Order>> GetProcessedAsync(string? searchText, SortOrder sortOrder, string? sortField, int skip, int pageSize)
        {
            if (string.IsNullOrEmpty(sortField) || !IndexViewModel<Order>.AllFieldNames.Contains(sortField))
            {
                return await GetAllAsync();
            }

            string sortOrderString = sortOrder == SortOrder.Descending ? "DESC" : "ASC";

            var param = new DynamicParameters();
            string conditions = GetSearchConditions(searchText, param);

            string sqlQuery = $"SELECT * FROM [Order] WHERE ({conditions}) ORDER BY {sortField} {sortOrderString} OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";

            using var dbConnection = this._context.CreateConnection();
            var orders = await dbConnection.QueryAsync<Order>(sqlQuery, param);

            return orders?.ToList() ?? new List<Order>();
        }

        public async Task<int> CountProcessedDataAsync(string? searchText)
        {
            var param = new DynamicParameters();
            string conditions = GetSearchConditions(searchText, param);

            string countQuery = $"SELECT COUNT(*) FROM [Order] WHERE ({conditions})";
            
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteScalarAsync<int>(countQuery, param);
        }

        private string GetSearchConditions(string? searchText, DynamicParameters param)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return "1=1";
            }

            var conditions = new List<string>();

            foreach (var fieldName in IndexViewModel<Order>.AllFieldNames)
            {
                var propertyType = typeof(Order).GetProperty(fieldName)?.PropertyType;

                if (propertyType == typeof(DateTime) && DateTime.TryParse(searchText, out var parsedDate))
                {
                    conditions.Add($"{fieldName} = @searchDate");
                    param.Add("@searchDate", parsedDate);
                }
                else if (propertyType == typeof(Guid) && Guid.TryParse(searchText, out var parsedId))
                {
                    conditions.Add($"{fieldName} = @searchId");
                    param.Add("@searchId", parsedId);
                }
            }

            return string.Join(" OR ", conditions);
        }
    }
}
