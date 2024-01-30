using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using CD_Disc_Store_React_ASP_NET_Core.Server.ViewModels;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exceptions;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Implementations
{
    public class DiscRepository(IDapperContext context) : IDiscRepository
    {
        private readonly IDapperContext _context = context;

        private const string DISC_NOT_FOUND_BY_ID_ERROR = "The disc with specified Id was not found";

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
            try
            {
                using IDbConnection dbConnection = this._context.CreateConnection();

                return await dbConnection.ExecuteAsync("INSERT INTO Disc (Id, [Name], Price, LeftOnStock, Rating, CoverImagePath, ImageStorageName) VALUES (@Id, @Name, @Price, @LeftOnStock, @Rating, @CoverImagePath, @ImageStorageName)", entity);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Error while adding a Disc to the database.", ex);
            }
        }

        public async Task<int> UpdateAsync(Disc entity)
        {
            try
            {
                Disc currentDisc;
                try
                {
                    currentDisc = await this.GetByIdAsync(entity.Id);

                    if (currentDisc is null || !IsEntityChanged(currentDisc, entity))
                    {
                        return 0;
                    }

                    using IDbConnection dbConnection = this._context.CreateConnection();
                    return await dbConnection.ExecuteAsync("UPDATE Disc SET Name = @Name, Price = @Price, LeftOnStock = @LeftOnStock, Rating = @Rating, CoverImagePath = @CoverImagePath, ImageStorageName = @ImageStorageName WHERE Id = @Id", entity);
                }
                catch (Exception ex)
                    when (ex is ArgumentNullException
                        || ex is NullReferenceException
                        || ex is NotFoundException)
                {
                    throw;
                }

            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Error while updating a Disc in the database.", ex);
            }
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

        public async Task<int> DeleteAsync(Guid id)
        {
            try
            {
                if (!await ExistsAsync(id))
                {
                    return 0;
                }

                using IDbConnection dbConnection = this._context.CreateConnection();
                return await dbConnection.ExecuteAsync($"DELETE FROM Disc WHERE Id = @Id", new { Id = id });
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Error while deleting a Disc from the database.", ex);
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteScalarAsync<bool>("SELECT COUNT(1) FROM Disc WHERE Id = @Id", new { Id = id });
        }


        public async Task<IReadOnlyList<Disc>> GetProcessedAsync(string? searchText, SortOrder sortOrder, string? sortField, int skip, int pageSize)
        {
            if (string.IsNullOrEmpty(sortField)
                || !GetAllViewModel<Disc>.AllFieldNames.Any(f => string.Equals(f, sortField, StringComparison.OrdinalIgnoreCase)))
            {
                return await GetAllAsync();
            }

            string sortOrderString = sortOrder == SortOrder.Descending ? "DESC" : "ASC";

            var param = new DynamicParameters();
            string conditions = GetSearchConditions(searchText, param);
            string sqlQuery = $"SELECT * FROM Disc WHERE ({conditions}) ORDER BY {sortField} {sortOrderString} OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";

            using IDbConnection dbConnection = this._context.CreateConnection();
            var discs = await dbConnection.QueryAsync<Disc>(sqlQuery, param);

            return discs?.ToList() ?? new List<Disc>();
        }

        public async Task<int> CountProcessedDataAsync(string? searchText)
        {
            var param = new DynamicParameters();
            string conditions = GetSearchConditions(searchText, param);

            string countQuery = $"SELECT COUNT(*) FROM Disc WHERE ({conditions})";

            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteScalarAsync<int>(countQuery, param);
        }

        private string GetSearchConditions(string? searchText, DynamicParameters param)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return "1=1";
            }

            var conditions = new List<string>();

            foreach (var fieldName in GetAllViewModel<Disc>.AllFieldNames)
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

        public async Task<IReadOnlyList<Film>> GetFilmsOnDiscAsync(Guid? id)
        {
            if (id is null)
            {
                throw new NullReferenceException(DISC_NOT_FOUND_BY_ID_ERROR);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            var query = @"
                SELECT
                    Film.*
                FROM
                    Disc
                JOIN DiscFilm ON Disc.Id = DiscFilm.IdDisc
                JOIN Film ON DiscFilm.IdFilm = Film.Id
                WHERE Disc.Id = @DiscId;";

            return (IReadOnlyList<Film>)await dbConnection.QueryAsync<Film>(query, new { DiscId = id });
        }

        public async Task<IReadOnlyList<Music>> GetMusicOnDiscAsync(Guid? id)
        {
            if (id is null)
            {
                throw new NullReferenceException(DISC_NOT_FOUND_BY_ID_ERROR);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            var query = @"
                SELECT
                    Music.*
                FROM
                    Disc
                JOIN DiscMusic ON Disc.Id = DiscMusic.IdDisc
                JOIN Music ON DiscMusic.IdMusic = Music.Id
                WHERE Disc.Id = @DiscId;";

            return (IReadOnlyList<Music>)await dbConnection.QueryAsync<Music>(query, new { DiscId = id });
        }
    }
}
