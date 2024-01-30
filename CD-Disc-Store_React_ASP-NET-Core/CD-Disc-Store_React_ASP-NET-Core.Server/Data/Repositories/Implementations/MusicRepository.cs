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
    public class MusicRepository(IDapperContext context) : IMusicRepository
    {
        private readonly IDapperContext _context = context;
        private const string MUSIC_NOT_FOUND_BY_ID_ERROR = "The music with specified Id was not found";

        public async Task<Music> GetByIdAsync(Guid? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id), MUSIC_NOT_FOUND_BY_ID_ERROR);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            var music = await dbConnection.QueryFirstOrDefaultAsync<Music>("SELECT * FROM Music WHERE Id = @Id", new { Id = id });
            return music ?? throw new NotFoundException(MUSIC_NOT_FOUND_BY_ID_ERROR);
        }

        public async Task<IReadOnlyList<Music>> GetAllAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            var music = await dbConnection.QueryAsync<Music>("SELECT * FROM Music");
            return (IReadOnlyList<Music>)music ?? new List<Music>();
        }

        public async Task<int> AddAsync(Music entity)
        {
            try
            {
                using IDbConnection dbConnection = this._context.CreateConnection();
                return await dbConnection.ExecuteAsync("INSERT INTO Music (Id, [Name], Genre, Artist, [Language], CoverImagePath, ImageStorageName) VALUES (@Id, @Name, @Genre, @Artist, @Language, @CoverImagePath, @ImageStorageName)", entity);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Error while adding a Music to the database.", ex);
            }
        }

        public async Task<int> UpdateAsync(Music entity)
        {
            try
            {
                Music currentMusic;
                try
                {
                    currentMusic = await this.GetByIdAsync(entity.Id);
                }
                catch (Exception ex)
                    when (ex is ArgumentNullException
                        || ex is NullReferenceException
                        || ex is NotFoundException)
                {
                    throw;
                }

                if (currentMusic != null && !IsEntityChanged(currentMusic, entity))
                {
                    return 0;
                }

                using IDbConnection dbConnection = this._context.CreateConnection();
                return await dbConnection.ExecuteAsync("UPDATE Music SET Name = @Name, Genre = @Genre, Artist = @Artist, Language = @Language, CoverImagePath = @CoverImagePath, ImageStorageName = @ImageStorageName WHERE Id = @Id", entity);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Error while updating a Music in the database.", ex);
            }
        }

        public bool IsEntityChanged(Music currentEntity, Music entity)
        {
            return currentEntity.Name != entity.Name
                || currentEntity.Genre != entity.Genre
                || currentEntity.Artist != entity.Artist
                || currentEntity.Language != entity.Language
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
                return await dbConnection.ExecuteAsync($"DELETE FROM Music WHERE Id = @Id", new { Id = id });
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Error while deleting a Music from the database.", ex);
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteScalarAsync<bool>("SELECT COUNT(1) FROM Music WHERE Id = @Id", new { Id = id });
        }

        public async Task<IReadOnlyList<Music>> GetProcessedAsync(string? searchText, SortOrder sortOrder, string? sortField, int skip, int pageSize)
        {
            if (string.IsNullOrEmpty(sortField)
               || !GetAllViewModel<Music>.AllFieldNames.Any(f => string.Equals(f, sortField, StringComparison.OrdinalIgnoreCase)))
            {
                return await GetAllAsync();
            }

            string sortOrderString = sortOrder == SortOrder.Descending ? "DESC" : "ASC";

            var param = new DynamicParameters();
            string conditions = GetSearchConditions(searchText, param);

            string sqlQuery = $"SELECT * FROM Music WHERE ({conditions}) ORDER BY {sortField} {sortOrderString} OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";

            using IDbConnection dbConnection = this._context.CreateConnection();
            var musics = await dbConnection.QueryAsync<Music>(sqlQuery, param);

            return musics?.ToList() ?? new List<Music>();
        }

        public async Task<int> CountProcessedDataAsync(string? searchText)
        {
            var param = new DynamicParameters();
            string conditions = GetSearchConditions(searchText, param);

            string countQuery = $"SELECT COUNT(*) FROM Music WHERE ({conditions})";

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

            foreach (var fieldName in GetAllViewModel<Music>.AllFieldNames)
            {
                var propertyType = typeof(Music).GetProperty(fieldName)?.PropertyType;

                if (propertyType == typeof(string))
                {
                    conditions.Add($"{fieldName} LIKE @searchText");
                    param.Add("@searchText", $"%{searchText}%");
                }
            }

            return string.Join(" OR ", conditions);
        }
    }
}
