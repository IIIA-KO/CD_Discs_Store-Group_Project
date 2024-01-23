using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exceptions;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Implementations
{
    public class DiscRepository : IDiscRepository
    {
        private readonly IDapperContext _context;

        private const string DISC_NOT_FOUND_BY_ID_ERROR = "The disc with specified Id was not found";

        public DiscRepository(IDapperContext context)
        {
            this._context = context;
        }

        public async Task<Disc> GetByIdAsync(Guid? id)
        {

            if (id is null)
            {
                throw new ArgumentNullException(nameof(id), DISC_NOT_FOUND_BY_ID_ERROR);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            var disc = await dbConnection.QueryFirstOrDefaultAsync<Disc>("SELECT * FROM Disc WHERE Id = @Id", new { Id = id });

            return disc ?? throw new NotFoundException(DISC_NOT_FOUND_BY_ID_ERROR);
        }

        public async Task<IReadOnlyList<Disc>> GetAllAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            var discs = await dbConnection.QueryAsync<Disc>("SELECT * FROM Disc");
            return (IReadOnlyList<Disc>)discs ?? new List<Disc>();
        }

        public async Task<int> AddAsync(Disc entity)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteAsync("INSERT INTO Disc (Id, [Name], Price, LeftOnStock, Rating, CoverImagePath, ImageStorageName) VALUES (@Id, @Name, @Price, @LeftOnStock, @Rating, @CoverImagePath, @ImageStorageName)", entity);
        }

        public async Task<int> UpdateAsync(Disc entity)
        {
            Disc currentDisc;
            try
            {
                currentDisc = await this.GetByIdAsync(entity.Id);
            }
            catch (Exception ex)
                when (ex is ArgumentNullException
                    || ex is NullReferenceException
                    || ex is NotFoundException)
            {
                throw;
            }

            if (currentDisc != null && !IsEntityChanged(currentDisc, entity))
            {
                return 0;
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteAsync("UPDATE Disc SET Name = @Name, Price = @Price, LeftOnStock = @LeftOnStock, Rating = @Rating, CoverImagePath = @CoverImagePath, ImageStorageName = @ImageStorageName WHERE Id = @Id", entity);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteAsync($"DELETE FROM Disc WHERE Id = @Id", new { Id = id });
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteScalarAsync<bool>("SELECT COUNT(1) FROM Disc WHERE Id = @Id", new { Id = id });
        }

        public bool IsEntityChanged(Disc currentEntity, Disc entity)
        {
            return currentEntity.Name != entity.Name
                || currentEntity.Price != entity.Price
                || currentEntity.LeftOnStock != entity.LeftOnStock
                || currentEntity.Rating != entity.Rating
                || currentEntity.CoverImagePath != entity.CoverImagePath
                || currentEntity.ImageStorageName != entity.ImageStorageName;
        }

		public async Task<IReadOnlyList<Disc>> GetProcessedAsync(string? searchText, SortOrder sortOrder, string? sortField, int skip, int pageSize)
		{
			if (string.IsNullOrEmpty(sortField) || !IndexViewModel<Film>.AllFieldNames.Contains(sortField))
			{
				return await GetAllAsync();
			}

			string sortOrderString = sortOrder == SortOrder.Descending ? "DESC" : "ASC";

			var param = new DynamicParameters();
			string conditions = GetSearchConditions(searchText, param);
			string sqlQuery = $"SELECT * FROM Film WHERE ({conditions}) ORDER BY {sortField} {sortOrderString} OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";

			using IDbConnection dbConnection = this._context.CreateConnection();
			var discs = await dbConnection.QueryAsync<Disc>(sqlQuery, param);

			return discs?.ToList() ?? new List<Disc>();
		}

		public async Task<int> CountProcessedDataAsync(string? searchText)
		{
			var param = new DynamicParameters();
			string conditions = GetSearchConditions(searchText, param);

			string countQuery = $"SELECT COUNT(*) FROM Film WHERE ({conditions})";

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

			foreach (var fieldName in IndexViewModel<Disc>.AllFieldNames)
			{
				var propertyType = typeof(Disc).GetProperty(fieldName)?.PropertyType;

				if (propertyType == typeof(string))
				{
					conditions.Add($"{fieldName} LIKE @searchText");
					param.Add("@searchText", $"%{searchText}%");
				}
				else if (propertyType == typeof(int) && int.TryParse(searchText, out var parsedInt))
				{
					conditions.Add($"{fieldName} = @searchInt");
					param.Add("@searchInt", parsedInt);
				}
				else if (propertyType == typeof(decimal) && int.TryParse(searchText, out var parsedDecimal))
				{
					conditions.Add($"{fieldName} = @searchDecimal");
					param.Add("@searchDecimal", parsedDecimal);
				}
			}

			return string.Join(" OR ", conditions);
		}
	}
}