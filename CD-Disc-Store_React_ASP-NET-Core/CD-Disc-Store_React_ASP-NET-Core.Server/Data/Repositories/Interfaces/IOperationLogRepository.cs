
namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces
{
	public interface IOperationLogRepository : IGenericRepository<OperationLog>
	{
		Task<IReadOnlyList<OperationLog>> GetByClientIdAsync(Guid? id);
		Task<IReadOnlyList<OperationLog>> GetByDiscIdAsync(Guid? id);
        Task<Guid> GetOperationTypeByNameAsync(string typeName); //<-- Temporary method, it'll be replaced in the near future as the project gets new updates
    }
}
