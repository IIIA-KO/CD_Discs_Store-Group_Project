using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
using Dapper;
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
            using IDbConnection dbConnection = this._context.CreateConnection();

            if (id is null)
            {
                throw new NullReferenceException(DISC_NOT_FOUND_BY_ID_ERROR);
            }

            return await dbConnection.QueryFirstOrDefaultAsync<Disc>($"SELECT * FROM Disc WHERE Id = @Id", new { Id = id });
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

            return await dbConnection.ExecuteAsync("INSERT INTO Disc (Id, [Name], Price, LeftOnStock, Rating) VALUES (@Id, @Name, @Price, @LeftOnStock, @Rating)", entity);
        }

        public async Task<int> UpdateAsync(Disc entity)
        {
            Disc currentDisc;
            try
            {
                currentDisc = await this.GetByIdAsync(entity.Id);
            }
            catch (NullReferenceException)
            {
                throw;
            }
            if (currentDisc != null && !IsEntityChanged(currentDisc, entity))
            {
                return 0;
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteAsync("UPDATE Disc SET Name = @Name, Price = @Price, LeftOnStock = @LeftOnStock, Rating = @Rating WHERE Id = @Id", entity);
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
                || currentEntity.Rating != entity.Rating;
        }
    }
}
