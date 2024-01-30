using Microsoft.Data.SqlClient;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        Task<Client> GetByUserId(string userId);
        Task<string> GetEmailAsync(Client client);
        Task<string> GetPhoneAsync(Client client);
    }
}
