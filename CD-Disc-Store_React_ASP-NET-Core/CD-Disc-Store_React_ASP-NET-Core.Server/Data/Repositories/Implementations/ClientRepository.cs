using Dapper;
using System.Data;
using static Dapper.SqlMapper;
using Microsoft.Data.SqlClient;
using CD_Disc_Store_React_ASP_NET_Core.Server.ViewModels;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exceptions;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Implementations
{
    public class ClientRepository(IDapperContext context) : IClientRepository
    {
        private readonly IDapperContext _context = context;

        private const string CLIENT_NOT_FOUND_BY_ID_ERROR = "The client with specified Id was not found.";
        private const string USER_DOES_NOT_EXIST = "The User with specified Id does not exist. Cannot Add Client.";

        public async Task<Client> GetByIdAsync(Guid? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id), CLIENT_NOT_FOUND_BY_ID_ERROR);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            var client = await dbConnection.QueryFirstOrDefaultAsync<Client>($"SELECT * FROM Client WHERE Id = @Id", new { Id = id });
            return client ?? throw new NotFoundException(CLIENT_NOT_FOUND_BY_ID_ERROR);
        }

        public async Task<Client> GetByUserId(string userId)
        {
            if(!await UserExistsAsync(userId))
            {
                throw new InvalidOperationException(USER_DOES_NOT_EXIST);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.QueryFirstOrDefaultAsync<Client>("SELECT * FROM Client WHERE UserId = @UserId", new { UserId = userId })
                ?? throw new DatabaseOperationException("Failed to get Client with specified User Id.");
        }

        public async Task<IReadOnlyList<Client>> GetAllAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            var clients = await dbConnection.QueryAsync<Client>("SELECT * FROM Client");
            return (IReadOnlyList<Client>)clients ?? new List<Client>();
        }

        public async Task<string> GetEmailAsync(Client client)
        {
            if (!await UserExistsAsync(client.UserId))
            {
                throw new InvalidOperationException(USER_DOES_NOT_EXIST);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.QueryFirstOrDefaultAsync<string>("SELECT Email FROM dbo.AspNetUsers WHERE Id = @Id", new { Id = client.UserId })
                ?? throw new DatabaseOperationException("Failed to get Email of Client with specified Id.");
        }

        public async Task<string> GetPhoneAsync(Client client)
        {
            if (!await UserExistsAsync(client.UserId))
            {
                throw new InvalidOperationException(USER_DOES_NOT_EXIST);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.QueryFirstOrDefaultAsync<string>("SELECT PhoneNumber FROM dbo.AspNetUsers WHERE Id = @Id", new { Id = client.UserId })
                ?? throw new DatabaseOperationException("Failed to get Phone Number of Client with specified Id.");
        }

        public async Task<int> AddAsync(Client entity)
        {
            try
            {
                if (!await UserExistsAsync(entity.UserId))
                {
                    throw new InvalidOperationException(USER_DOES_NOT_EXIST);
                }

                using IDbConnection dbConnection = this._context.CreateConnection();
                dbConnection.Open();
                
                using IDbTransaction transaction = dbConnection.BeginTransaction();

                try
                {
                    var result = await dbConnection.ExecuteAsync("INSERT INTO Client (Id, UserId, [Address], City, BirthDay, MarriedStatus, Sex, HasChild) " +
                        "VALUES (@Id, @UserId, @Address, @City, @BirthDay, @MarriedStatus, @Sex, @HasChild)", entity, transaction);

                    transaction.Commit();
                    return result;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Error while adding a Client to the database.", ex);
            }
        }

        private async Task<bool> UserExistsAsync(string userId)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteScalarAsync<bool>("SELECT COUNT(1) FROM dbo.AspNetUsers WHERE Id = @Id", new { Id = userId });
        }

        public async Task<int> UpdateAsync(Client entity)
        {
            try
            {
                Client currentClient;
                try
                {
                    currentClient = await this.GetByIdAsync(entity.Id);

                    if (currentClient is null || !IsEntityChanged(currentClient, entity))
                    {
                        return 0;
                    }

                    using IDbConnection dbConnection = this._context.CreateConnection();

                    return await dbConnection.ExecuteAsync("UPDATE Client SET FirstName = @FirstName, LastName = @LastName, Address = @Address, City = @City, " +
                    "ContactPhone = @ContactPhone, ContactMail = @ContactMail, BirthDay = @BirthDay, MarriedStatus = @MarriedStatus, Sex = @Sex, HasChild = @HasChild WHERE Id = @Id", entity);
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
                throw new DatabaseOperationException("Error while updating a Client in the database.", ex);
            }
        }

        public bool IsEntityChanged(Client currentEntity, Client entity)
        {
            return currentEntity.Address != entity.Address
                || currentEntity.City != entity.City
                || currentEntity.BirthDay != entity.BirthDay
                || currentEntity.MarriedStatus != entity.MarriedStatus
                || currentEntity.Sex != entity.Sex
                || currentEntity.HasChild != entity.HasChild;
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
                return await dbConnection.ExecuteAsync($"DELETE FROM Client WHERE Id = @Id", new { Id = id });
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Error while deleting a Client from the database.", ex);
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteScalarAsync<bool>("SELECT COUNT(1) FROM Client WHERE Id = @Id", new { Id = id });
        }

        public async Task<IReadOnlyList<Client>> GetProcessedAsync(string? searchText, SortOrder sortOrder, string? sortField, int skip, int pageSize)
        {
            if (string.IsNullOrEmpty(sortField)
                || !GetAllViewModel<Client>.AllFieldNames.Any(f => string.Equals(f, sortField, StringComparison.OrdinalIgnoreCase)))
            {
                return await GetAllAsync();
            }

            string sortOrderString = sortOrder == SortOrder.Descending ? "DESC" : "ASC";

            var param = new DynamicParameters();
            string conditions = GetSearchConditions(searchText, param);

            string sqlQuery = $"SELECT * FROM Client WHERE ({conditions}) ORDER BY {sortField} {sortOrderString} OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";

            using IDbConnection dbConnection = this._context.CreateConnection();
            var clients = await dbConnection.QueryAsync<Client>(sqlQuery, param);

            return clients?.ToList() ?? new List<Client>();
        }

        public async Task<int> CountProcessedDataAsync(string? searchText)
        {
            var param = new DynamicParameters();
            string conditions = GetSearchConditions(searchText, param);

            string countQuery = $"SELECT COUNT(*) FROM Client WHERE ({conditions})";

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

            foreach (var fieldName in GetAllViewModel<Client>.AllFieldNames)
            {
                var propertyType = typeof(Client).GetProperty(fieldName)?.PropertyType;

                if (propertyType == typeof(string))
                {
                    conditions.Add($"{fieldName} LIKE @searchText");
                    param.Add("@searchText", $"%{searchText}%");
                }
                else if (propertyType == typeof(DateTime) && DateTime.TryParse(searchText, out var parsedDate))
                {
                    conditions.Add($"{fieldName} = @searchDate");
                    param.Add("@searchDate", parsedDate);
                }
                else if (propertyType == typeof(bool) && bool.TryParse(searchText, out var parsedBool))
                {
                    conditions.Add($"{fieldName} = @searchBool");
                    param.Add("@searchBool", parsedBool);
                }
            }

            return string.Join(" OR ", conditions);
        }
    }
}
