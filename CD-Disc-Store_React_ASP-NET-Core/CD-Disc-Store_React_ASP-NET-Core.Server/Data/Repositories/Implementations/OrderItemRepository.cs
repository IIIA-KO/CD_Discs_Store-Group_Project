using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exceptions;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Implementations
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly IDapperContext _context;
        private readonly IOrderRepository _orderRepository;
        private readonly IDiscRepository _discRepository;

        private const string ORDER_ITEM_NOT_FOUND_BY_ID = "The order item with specified Id was not found.";
        private const string ORDER_DOES_NOT_EXIST = "The Order with specified Id does not exist. Cannot Add Order Item";
        private const string DISC_DOES_NOT_EXIST = "The Disc with specified Id does not exist. Cannot Add Order Item";

        public OrderItemRepository(IDapperContext context, IOrderRepository orderRepository, IDiscRepository discRepository)
        {
            this._context = context;
            this._orderRepository = orderRepository;
            this._discRepository = discRepository;
        }

        public async Task<OrderItem> GetByIdAsync(Guid? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id), ORDER_ITEM_NOT_FOUND_BY_ID);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            var orderItem = await dbConnection.QueryFirstOrDefaultAsync<OrderItem>("SELECT * FROM OrderItem WHERE Id = @Id", new { Id = id });
            return orderItem ?? throw new NotFoundException();
        }
        public async Task<IReadOnlyList<OrderItem>> GetAllAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            var orderItems = await dbConnection.QueryAsync<OrderItem>("SELECT * FROM OrderItem");
            return (IReadOnlyList<OrderItem>)orderItems ?? new List<OrderItem>();
        }

        public async Task<int> AddAsync(OrderItem entity)
        {
            if (!await this._orderRepository.ExistsAsync(entity.IdOrder))
            {
                throw new InvalidOperationException(ORDER_DOES_NOT_EXIST);
            }

            if (!await this._discRepository.ExistsAsync(entity.IdDisc))
            {
                throw new InvalidOperationException(DISC_DOES_NOT_EXIST);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            dbConnection.Open();

            using IDbTransaction transaction = dbConnection.BeginTransaction();

            try
            {
                var result = await dbConnection.ExecuteAsync(
                    "INSERT INTO Orderitem (Id, IdOrder, IdDisc, Quantity) VALUES (@Id, @IdOrder, @IdDisc, @Quantity)",
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

        public async Task<int> UpdateAsync(OrderItem entity)
        {
            OrderItem currentItem;

            try
            {
                currentItem = await this.GetByIdAsync(entity.Id);
            }
            catch (Exception ex)
                when (ex is ArgumentNullException
                    || ex is NullReferenceException
                    || ex is NotFoundException)
            {
                throw;
            }

            if (currentItem != null && !IsEntityChanged(currentItem, entity))
            {
                return 0;
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteAsync("UPDATE OrderItem SET IdOrder = @IdOrder, IdDisc = @IdDisc, Quantity = @Quantity WHERE Id = @Id", entity);
        }
        public bool IsEntityChanged(OrderItem currentEntity, OrderItem entity)
        {
            return currentEntity.IdOrder != entity.IdOrder
                || currentEntity.IdDisc != entity.IdDisc
                || currentEntity.Quantity != entity.Quantity;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            if (!await this.ExistsAsync(id))
            {
                return 0;
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteAsync("DELETE FROM OrderItem WHERE Id = @Id", new { Id = id });
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteScalarAsync<bool>("SELECT COUNT(1) FROM OrderItem WHERE Id = @Id", new { Id = id });
        }

        public async Task<IReadOnlyList<OrderItem>> GetProcessedAsync(string? searchText, SortOrder sortOrder, string? sortField, int skip, int pageSize)
        {
            if (string.IsNullOrEmpty(sortField) || !IndexViewModel<OrderItem>.AllFieldNames.Contains(sortField))
            {
                return await GetAllAsync();
            }

            string sortOrderString = sortOrder == SortOrder.Descending ? "DESC" : "ASC";

            var param = new DynamicParameters();
            string conditions = GetSearchConditions(searchText, param);
            string sqlQuery = $"SELECT * FROM OrderItem WHERE ({conditions}) ORDER BY {sortField} {sortOrderString} OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";

            using IDbConnection dbConnection = this._context.CreateConnection();
            var orderItems = await dbConnection.QueryAsync<OrderItem>(sqlQuery, param);

            return orderItems?.ToList() ?? new List<OrderItem>();
        }

        public async Task<int> CountProcessedDataAsync(string? searchText)
        {
            var param = new DynamicParameters();
            string conditions = GetSearchConditions(searchText, param);

            string countQuery = $"SELECT COUNT(*) FROM OrderItem WHERE ({conditions})";
            
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

                if (propertyType == typeof(int) && int.TryParse(searchText, out var parsedInt))
                {
                    conditions.Add($"{fieldName} = @searchInt");
                    param.Add("@searchInt", parsedInt);
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
