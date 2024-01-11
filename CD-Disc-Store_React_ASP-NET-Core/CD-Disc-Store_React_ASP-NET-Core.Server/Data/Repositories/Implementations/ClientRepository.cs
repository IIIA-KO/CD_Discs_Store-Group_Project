using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exceptions;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Implementations
{
    public class ClientRepository : IClientRepository
    {
        private readonly IDapperContext _context;

        private const string CLIENT_NOT_FOUND_BY_ID_ERROR = "The client with specified Id was not found.";

        public ClientRepository(IDapperContext context)
        {
            this._context = context;
        }

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

        public async Task<IReadOnlyList<Client>> GetAllAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            var clients = await dbConnection.QueryAsync<Client>("SELECT * FROM Client");
            return (IReadOnlyList<Client>)clients ?? new List<Client>();
        }

        public async Task<int> AddAsync(Client entity)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteAsync("INSERT INTO Client (Id, FirstName, LastName, [Address], City, ContactPhone, ContactMail, BirthDay, MarriedStatus, Sex, HasChild) " +
            "VALUES (@Id, @FirstName, @LastName, @Address, @City, @ContactPhone, @ContactMail, @BirthDay, @MarriedStatus, @Sex, @HasChild)", entity);
        }

        public async Task<int> UpdateAsync(Client entity)
        {
            Client currentClient;
            try
            {
                currentClient = await this.GetByIdAsync(entity.Id);
            }
            catch (Exception ex)
                when (ex is ArgumentNullException
                    || ex is NullReferenceException
                    || ex is NotFoundException)
            {
                throw;
            }

            if (currentClient != null && !IsEntityChanged(currentClient, entity))
            {
                return 0;
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteAsync("UPDATE Client SET FirstName = @FirstName, LastName = @LastName, Address = @Address, City = @City, " +
            "ContactPhone = @ContactPhone, ContactMail = @ContactMail, BirthDay = @BirthDay, MarriedStatus = @MarriedStatus, Sex = @Sex, HasChild = @HasChild WHERE Id = @Id", entity);
        }

        public bool IsEntityChanged(Client currentEntity, Client entity)
        {
            return currentEntity.FirstName != entity.FirstName
                || currentEntity.LastName != entity.LastName
                || currentEntity.Address != entity.Address
                || currentEntity.City != entity.City
                || currentEntity.ContactPhone != entity.ContactPhone
                || currentEntity.ContactMail != entity.ContactMail
                || currentEntity.BirthDay != entity.BirthDay
                || currentEntity.MarriedStatus != entity.MarriedStatus
                || currentEntity.Sex != entity.Sex
                || currentEntity.HasChild != entity.HasChild;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            if (!await ExistsAsync(id))
            {
                return 0;
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteAsync($"DELETE FROM Client WHERE Id = @Id", new { Id = id });
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteScalarAsync<bool>("SELECT COUNT(1) FROM Client WHERE Id = @Id", new { Id = id });
        }

        public async Task<IReadOnlyList<Client>> GetProcessedAsync(string? searchText, SortOrder sortOrder, string? sortField, int skip, int pageSize)
        {
            if (string.IsNullOrEmpty(sortField) || !IndexViewModel<Client>.AllFieldNames.Contains(sortField))
            {
                return await GetAllAsync();
            }

            string sortOrderString = sortOrder == SortOrder.Descending ? "DESC" : "ASC";

            var param = new DynamicParameters();
            string conditions = GetSearchConditions(searchText, param);

            string sqlQuery = $"SELECT * FROM Client WHERE ({conditions}) ORDER BY {sortField} {sortOrderString} OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";

            using var dbConnection = this._context.CreateConnection();
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
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return "1=1";
            }

            var conditions = new List<string>();

            foreach (var fieldName in IndexViewModel<Client>.AllFieldNames)
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