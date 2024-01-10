using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exceptions;
using Dapper;
using System.Data;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Implementations
{
    public class FilmRepositiry : IFilmRepository
    {
        private readonly IDapperContext _context;
        private const string FILM_NOT_FOUND_BY_ID_ERROR = "The film with specified Id was not found";

        public FilmRepositiry(IDapperContext context)
        {
            this._context = context;
        }

        public async Task<Film> GetByIdAsync(Guid? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(FILM_NOT_FOUND_BY_ID_ERROR);
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
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteAsync("INSERT INTO Film ([Name], Genre, Producer, MainRole, AgeLimit) VALUES (@Name, @Genre, @Producer, @MainRole, @AgeLimit)", entity);
        }

        public async Task<int> UpdateAsync(Film entity)
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

            if(currentFilm != null && !IsEntityChanged(currentFilm, entity))
            {
                return 0;
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteAsync("UPDATE Film SET Name = @Name, Genre = @Genre, Producer = @Producer, MainRole = @MainRole, AgeLimit = @AgeLimit WHERE Id = @Id", entity);
        }

        public bool IsEntityChanged(Film currentEntity, Film entity)
        {
            return currentEntity.Name != entity.Name
                || currentEntity.Genre != entity.Genre
                || currentEntity.Producer != entity.Producer
                || currentEntity.MainRole != entity.MainRole
                || currentEntity.AgeLimit != entity.AgeLimit;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            if (!await ExistsAsync(id))
            {
                return 0;
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteAsync($"DELETE FROM Film WHERE Id = @Id", new { Id = id });
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteScalarAsync<bool>("SELECT COUNT(1) FROM Film WHERE Id = @Id", new { Id = id });
        }
    }
}
