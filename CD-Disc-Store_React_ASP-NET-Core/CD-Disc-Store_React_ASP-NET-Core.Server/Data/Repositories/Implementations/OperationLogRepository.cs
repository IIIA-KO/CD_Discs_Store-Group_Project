using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
using Dapper;
using System.Data;
using static Dapper.SqlMapper;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Implementations
{
	public class OperationLogRepository : IOperationLogRepository
	{
		private readonly IDapperContext _context;

		private const string OPERATION_NOT_FOUND_BY_ID_ERROR = "The operation with specified Id was not found";

        public OperationLogRepository(IDapperContext context)
        {
			this._context = context;
        }

		public async Task<int> AddAsync(OperationLog operationLog)
		{
			using IDbConnection dbConnection = this._context.CreateConnection();
			dbConnection.Open();

			using IDbTransaction transaction = dbConnection.BeginTransaction();

			try
			{
				var result = await dbConnection.QueryAsync<Guid>(
					"INSERT INTO [Order] (OrderDateTime, IdClient) OUTPUT INSERTED.Id VALUES (@OperationDateTimeStart, @ClientId)",
					new 
					{ 
						OperationDateTimeStart = operationLog.OperationDateTimeStart, 
						ClientId = operationLog.ClientId 
					},
					transaction
				);

				Guid orderId = result.Single();

				await dbConnection.ExecuteAsync(
					"INSERT INTO OrderItem (IdOrder, IdDisc, Quantity) VALUES (@IdOrder, @IdDisc, @Quantity)",
					new 
					{ 
						IdOrder = orderId, 
						IdDisc = operationLog.DiscId,
						Quantity = operationLog.Quantity 
					},
					transaction
				);

				var operationTypeId = await dbConnection.ExecuteScalarAsync<Guid>(
					"SELECT Id FROM OperationType WHERE TypeName = @TypeName",
					new { TypeName = operationLog.OperationType },
					transaction
				);

				if (operationTypeId == Guid.Empty)
				{
					operationTypeId = await dbConnection.ExecuteScalarAsync<Guid>(
						"INSERT INTO OperationType(TypeName) VALUES (@OperationType) SELECT CAST(SCOPE_IDENTITY() as uniqueidentifier)",
						new { OperationType = operationLog.OperationType }, 
						transaction
					);
				}

				await dbConnection.ExecuteAsync(
					"INSERT INTO OperationLog (OperationType, OperationDateTimeStart, OperationDateTimeEnd, IdOrder) " +
					"VALUES (@OperationType, @OperationDateTimeStart, @OperationDateTimeEnd, @IdOrder)",
					new
					{ 
						OperationType = operationTypeId, 
						OperationDateTimeStart = operationLog.OperationDateTimeStart,
						OperationDateTimeEnd = operationLog.OperationDateTimeEnd, 
						IdOrder = orderId 
					},
					transaction
				);

				transaction.Commit();
				return 1;
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}
		}


		public async Task<int> DeleteAsync(Guid id)
		{
			using IDbConnection dbConnection = this._context.CreateConnection();
			return await dbConnection.ExecuteAsync($"DELETE FROM OperationLog WHERE Id = @Id", new { Id = id });
		}

		public async Task<bool> ExistsAsync(Guid id)
		{
			using IDbConnection dbConnection = this._context.CreateConnection();
			return await dbConnection.ExecuteScalarAsync<bool>("SELECT COUNT(1) FROM OperationLog WHERE Id = @Id", new { Id = id });
		}

		public async Task<IReadOnlyList<OperationLog>> GetAllAsync()
		{
			using IDbConnection dbConnection = this._context.CreateConnection();

			var allIds = await dbConnection.QueryAsync<Guid>("SELECT Id FROM OperationLog");

			var result = new List<OperationLog>();
			foreach (var id in allIds)
			{
				var operationLog = await GetByIdAsync(id);
				if (operationLog != null)
				{
					result.Add(operationLog);
				}
			}

			return result;
		}

		public async Task<OperationLog> GetByIdAsync(Guid? id)
		{
			var result = await GetClientsByAsync(id, $"WHERE OperationLog.Id = @Id");
			return result.FirstOrDefault();
		}


		public async Task<IReadOnlyList<OperationLog>> GetByClientIdAsync(Guid? id)
		{
			return await GetClientsByAsync(id, $"WHERE Client.Id = @Id");
		}

		public async Task<IReadOnlyList<OperationLog>> GetByDiscIdAsync(Guid? id)
		{
			return await GetClientsByAsync(id, $"WHERE Disc.Id = @Id");
		}

		private async Task<IReadOnlyList<OperationLog>> GetClientsByAsync(Guid? id, string condition = "")
		{
			using IDbConnection dbConnection = this._context.CreateConnection();

			if (id is null)
			{
				throw new NullReferenceException(OPERATION_NOT_FOUND_BY_ID_ERROR);
			}

			var operations = await dbConnection.QueryAsync<OperationLog>(
				$"SELECT OperationLog.Id, OperationLog.OperationDateTimeStart, OperationLog.OperationDateTimeEnd, " +
				$"Client.Id AS ClientId, Disc.Id AS DiscId, OperationType.TypeName AS OperationType, OrderItem.Quantity " +
				$"FROM OperationLog " +
				$"JOIN [Order] ON OperationLog.IdOrder = [Order].Id " +
				$"JOIN OrderItem ON [Order].Id = OrderItem.IdOrder " +
				$"JOIN Client ON [Order].IdClient = Client.Id " +
				$"JOIN Disc ON OrderItem.IdDisc = Disc.Id " +
				$"JOIN OperationType ON OperationLog.OperationType = OperationType.Id " +
				condition, new { Id = id });

			return operations?.ToList() ?? new List<OperationLog>();
		}


		public bool IsEntityChanged(OperationLog currentEntity, OperationLog entity)
		{
			return currentEntity.Id == entity.Id
				|| currentEntity.OperationDateTimeStart == entity.OperationDateTimeStart
				|| currentEntity.OperationDateTimeEnd == entity.OperationDateTimeEnd
				|| currentEntity.OperationType == entity.OperationType
				|| currentEntity.ClientId.CompareTo(entity.ClientId) == 0
				|| currentEntity.DiscId.CompareTo(entity.DiscId) == 0
				|| currentEntity.Quantity == entity.Quantity;
		}

		public async Task<int> UpdateAsync(OperationLog operationLog)
		{
			OperationLog currentOperation;
			try
			{
				currentOperation = await this.GetByIdAsync(operationLog.Id);
			}
			catch (NullReferenceException)
			{
				throw;
			}

			if (currentOperation != null && !IsEntityChanged(currentOperation, operationLog))
			{
				return 0;
			}


			using IDbConnection dbConnection = this._context.CreateConnection();
			dbConnection.Open();

			using IDbTransaction transaction = dbConnection.BeginTransaction();

			try
			{
				var operationTypeId = await dbConnection.ExecuteScalarAsync<Guid>(
					"SELECT Id FROM OperationType WHERE TypeName = @TypeName",
					new { TypeName = operationLog.OperationType },
					transaction
				);

				var orderId = await dbConnection.ExecuteScalarAsync<Guid>(
					"SELECT IdOrder FROM OperationLog WHERE Id = @Id",
					new { Id = operationLog.Id },
					transaction
				);

				await dbConnection.ExecuteAsync(
					"UPDATE [Order] SET OrderDateTime = @OrderDateTime, IdClient = @ClientId WHERE Id = @Id",
					new 
					{ 
						OrderDateTime = operationLog.OperationDateTimeStart, 
						ClientId = operationLog.ClientId,
						Id = orderId
					},
					transaction
				);

				await dbConnection.ExecuteAsync(
					"UPDATE OrderItem SET IdDisc = @IdDisc, Quantity = @Quantity WHERE IdOrder = @IdOrder",
					new 
					{ 
						IdOrder = orderId, 
						IdDisc = operationLog.DiscId,
						Quantity = operationLog.Quantity 
					},
					transaction
				);

				await dbConnection.ExecuteAsync(
					"UPDATE OperationLog SET OperationType = @OperationTypeId, OperationDateTimeStart = @OperationDateTimeStart, " +
					"OperationDateTimeEnd = @OperationDateTimeEnd WHERE Id = @Id",
					new
					{
						OperationTypeId = operationTypeId,
						OperationDateTimeStart = operationLog.OperationDateTimeStart,
						OperationDateTimeEnd = operationLog.OperationDateTimeEnd,
						Id = operationLog.Id
					},
					transaction
				);

				transaction.Commit();
				return 1;
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}

		}

	}

}
