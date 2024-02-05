using Dapper;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Processors;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly IDapperContext _context;
        private readonly ProcessableViewModelProcessor<TEntity> _processor;

        public GenericRepository(IDapperContext context, ProcessableViewModelProcessor<TEntity> processor)
        {
            this._context = context;
            this._processor = processor;
        }

        protected static string GetNotFoundErrorMessage() =>
            $"The {GetTableName()} with specified Id was not found.";

        private static string GetTableName() =>
            typeof(TEntity).Name;

        public async Task<TEntity> GetByIdAsync(Guid? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            using IDbConnection dbConnection = _context.CreateConnection();
            var entity = await dbConnection.QueryFirstOrDefaultAsync<TEntity>($"SELECT * FROM {GetTableName()} WHERE Id = @Id", new { Id = id });
            return entity ?? throw new NotFoundException(GetNotFoundErrorMessage());
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            var entities = await dbConnection.QueryAsync<TEntity>($"SELECT * FROM {GetTableName()}");
            return entities?.ToList() ?? [];
        }

        public async Task<int> AddAsync(TEntity entity)
        {
            try
            {
                using IDbConnection dbConnection = this._context.CreateConnection();
                return await dbConnection.ExecuteAsync(GetInsertSql(), entity);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException($"Error while adding the {GetTableName()} to the database.", ex);
            }
        }

        private string GetInsertSql()
        {
            var properties = GetPropertiesWithoutKey();
            return $"INSERT INTO {GetTableName()} ({string.Join(", ", properties)}) VALUES (@{string.Join(", @", properties)})";
        }

        public async Task<int> UpdateAsync(TEntity entity)
        {
            try
            {
                if (entity is null)
                {
                    return 0;
                }

                using IDbConnection dbConnection = this._context.CreateConnection();
                return await dbConnection.ExecuteAsync(GetUpdateSql(), entity);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException($"Error while updating the {GetTableName()} int the database.", ex);
            }
        }

        private string GetUpdateSql()
        {
            var properties = GetPropertiesWithoutKey();
            return $"UPDATE {GetTableName()} SET {string.Join(", ", properties.Select(p => $"{p} = @{p}"))} WHERE Id = @Id";
        }

        private IEnumerable<string> GetPropertiesWithoutKey()
        {
            return typeof(TEntity)
                .GetProperties()
                .Where(p => !Attribute.IsDefined(p, typeof(NotMappedAttribute)))
                .Select(p => p.Name)
                .ToList();
        }


        public async Task<int> DeleteAsync(Guid id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            var result = await dbConnection.ExecuteAsync($"DELETE FROM {GetTableName()} WHERE Id = @Id", new { Id = id });
            return result;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            var count = await dbConnection.ExecuteScalarAsync<int>($"SELECT COUNT(1) FROM {GetTableName()} WHERE Id = @Id", new { Id = id });
            return count > 0;
        }

        public async Task<IReadOnlyList<TEntity>> GetProcessedAsync(ProcessableViewModel<TEntity> processable)
        {
            var param = new DynamicParameters();
            var sqlQuery = this._processor.GetSqlQuery(processable, param);

            using IDbConnection dbConnection = this._context.CreateConnection();
            var items = await dbConnection.QueryAsync<TEntity>(sqlQuery, param);

            return items.ToList() ?? [];
        }
    }
}
