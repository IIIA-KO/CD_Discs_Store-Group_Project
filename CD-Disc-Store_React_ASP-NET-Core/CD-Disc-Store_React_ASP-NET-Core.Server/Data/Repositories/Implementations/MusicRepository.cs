using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
using Dapper;
using System.Data;


namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Implementations
{
    public class MusicRepository : IMusicRepository 
    {
        private readonly IDapperContext _context;
        private const string MUSIC_NOT_FOUND_BY_ID_ERROR = "The music with specified Id was not found";

        public MusicRepository(IDapperContext context)
        {
            this._context = context;
        }

        public async Task<Music> GetByIdAsync(Guid? id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            if (id is null)
            {
                throw new NullReferenceException(MUSIC_NOT_FOUND_BY_ID_ERROR);
            }

            return await dbConnection.QueryFirstOrDefaultAsync<Music>("SELECT * FROM Music WHERE Id = @Id", new { Id = id })
                ?? throw new NullReferenceException(MUSIC_NOT_FOUND_BY_ID_ERROR);
        }

        public async Task<IReadOnlyList<Music>> GetAllAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            var music = await dbConnection.QueryAsync<Music>("SELECT * FROM Music");
            return (IReadOnlyList<Music>)music ?? new List<Music>();
        }

        public async Task<int> AddAsync(Music entity)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteAsync("INSERT INTO Music ([Name], Genre, Artist, [Language]) VALUES (@Name, @Genre, @Artist, @Language)", entity);
        }

        public async Task<int> UpdateAsync(Music entity)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteAsync("UPDATE Music SET Name = @Name, Genre = @Genre, Artist = @Artist, Language = @Language WHERE Id = @Id", entity);
        }

        public bool IsEntityChanged(Music currentEntity, Music entity)
        {
            return currentEntity.Name != entity.Name
                || currentEntity.Genre != entity.Genre
                || currentEntity.Artist != entity.Artist
                || currentEntity.Language != entity.Language;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            if (!await ExistsAsync(id))
            {
                return 0;
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteAsync($"DELETE FROM Music WHERE Id = @Id", new { Id = id });
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteScalarAsync<bool>("SELECT COUNT(1) FROM Music WHERE Id = @Id", new { Id = id });
        }
    }
}
