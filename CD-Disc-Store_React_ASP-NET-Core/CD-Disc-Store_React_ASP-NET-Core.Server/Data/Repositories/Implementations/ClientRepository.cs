using Dapper;
using System.Data;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Processors;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Implementations
{
    public class ClientRepository(IDapperContext context, Processor<Client> processor)
        : GenericRepository<Client>(context, processor), IClientRepository
    {
        public async Task<Client> GetByUserIdAsync(string userId)
        {
            if (!await UserExistsAsync(userId))
            {
                throw new InvalidOperationException(GetNotFoundErrorMessage());
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.QueryFirstOrDefaultAsync<Client>("SELECT * FROM Client WHERE UserId = @UserId", new { UserId = userId })
                ?? throw new DatabaseOperationException("Failed to get Client with specified User Id.");
        }
        public async Task<string> GetEmailAsync(Client client)
        {
            if (!await UserExistsAsync(client.UserId))
            {
                throw new InvalidOperationException(GetNotFoundErrorMessage());
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.QueryFirstOrDefaultAsync<string>("SELECT Email FROM dbo.AspNetUsers WHERE Id = @Id", new { Id = client.UserId })
                ?? throw new DatabaseOperationException("Failed to get Email of Client with specified Id.");
        }

        public async Task<string> GetPhoneAsync(Client client)
        {
            if (!await UserExistsAsync(client.UserId))
            {
                throw new InvalidOperationException(GetNotFoundErrorMessage());
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.QueryFirstOrDefaultAsync<string>("SELECT PhoneNumber FROM dbo.AspNetUsers WHERE Id = @Id", new { Id = client.UserId })
                ?? throw new DatabaseOperationException("Failed to get Phone Number of Client with specified Id.");
        }

        public async Task<bool> UserExistsAsync(string userId)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteScalarAsync<bool>("SELECT COUNT(1) FROM dbo.AspNetUsers WHERE Id = @Id", new { Id = userId });
        }
    }
}
