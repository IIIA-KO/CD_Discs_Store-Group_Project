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
    public class FilmRepositiry(IDapperContext context) : IFilmRepository
    {
        private readonly IDapperContext _context = context;
        private const string FILM_NOT_FOUND_BY_ID_ERROR = "The film with specified Id was not found";

        public async Task<Film> GetByIdAsync(Guid? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id), FILM_NOT_FOUND_BY_ID_ERROR);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            var film = await dbConnection.QueryFirstOrDefaultAsync<Film>("SELECT * FROM Film WHERE Id = @Id", new { Id = id });
            return film ?? throw new NotFoundException(FILM_NOT_FOUND_BY_ID_ERROR);
        }

        public async Task<IReadOnlyList<Film>> GetAllAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            var films = await dbConnection.QueryAsync<Film>("SELECT * FROM Film");
            return (IReadOnlyList<Film>)films ?? new List<Film>();
        }

        public async Task<int> AddAsync(Film entity)
        {
            try
            {
                using IDbConnection dbConnection = this._context.CreateConnection();
                return await dbConnection.ExecuteAsync("INSERT INTO Film (Id, [Name], Genre, Producer, MainRole, AgeLimit, CoverImagePath, ImageStorageName) VALUES (@Id, @Name, @Genre, @Producer, @MainRole, @AgeLimit, @CoverImagePath, @ImageStorageName)", entity);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Error while adding a Film to the database.", ex);
            }
        }

        public async Task<int> UpdateAsync(Film entity)
        {
            try
            {
                Film currentFilm;
                try
                {
                    currentFilm = await this.GetByIdAsync(entity.Id);
                }
                catch (Exception ex)
                    when (ex is ArgumentNullException
                        || ex is NullReferenceException
                        || ex is NotFoundException)
                {
                    throw;
                }

                if (currentFilm != null && !IsEntityChanged(currentFilm, entity))
                {
                    return 0;
                }

                using IDbConnection dbConnection = this._context.CreateConnection();
                return await dbConnection.ExecuteAsync("UPDATE Film SET Name = @Name, Genre = @Genre, Producer = @Producer, MainRole = @MainRole, AgeLimit = @AgeLimit, CoverImagePath = @CoverImagePath, ImageStorageName = @ImageStorageName WHERE Id = @Id", entity);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Error while updating a Film in the database.", ex);
            }
        }

        public bool IsEntityChanged(Film currentEntity, Film entity)
        {
            return currentEntity.Name != entity.Name
                || currentEntity.Genre != entity.Genre
                || currentEntity.Producer != entity.Producer
                || currentEntity.MainRole != entity.MainRole
                || currentEntity.AgeLimit != entity.AgeLimit
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
                return await dbConnection.ExecuteAsync($"DELETE FROM Film WHERE Id = @Id", new { Id = id });
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Error while deleting a Film from the database.", ex);
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteScalarAsync<bool>("SELECT COUNT(1) FROM Film WHERE Id = @Id", new { Id = id });
        }

        public async Task<IReadOnlyList<Film>> GetProcessedAsync(string? searchText, SortOrder sortOrder, string? sortField, int skip, int pageSize)
        {
            if (string.IsNullOrEmpty(sortField)
               || !GetAllViewModel<Film>.AllFieldNames.Any(f => string.Equals(f, sortField, StringComparison.OrdinalIgnoreCase)))
            {
                return await GetAllAsync();
            }

            string sortOrderString = sortOrder == SortOrder.Descending ? "DESC" : "ASC";

            var param = new DynamicParameters();
            string conditions = GetSearchConditions(searchText, param);
            string sqlQuery = $"SELECT * FROM Film WHERE ({conditions}) ORDER BY {sortField} {sortOrderString} OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";

            using IDbConnection dbConnection = this._context.CreateConnection();
            var films = await dbConnection.QueryAsync<Film>(sqlQuery, param);

            return films?.ToList() ?? new List<Film>();
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
            if (string.IsNullOrEmpty(searchText))
            {
                return "1=1";
            }

            var conditions = new List<string>();

            foreach (var fieldName in GetAllViewModel<Film>.AllFieldNames)
            {
                var propertyType = typeof(Film).GetProperty(fieldName)?.PropertyType;

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
            }

            return string.Join(" OR ", conditions);
        }
    }
}
