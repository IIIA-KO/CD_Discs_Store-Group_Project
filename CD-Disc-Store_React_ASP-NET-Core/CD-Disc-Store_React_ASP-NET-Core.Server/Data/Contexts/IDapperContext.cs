using System.Data;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts
{
    public interface IDapperContext
    {
        public IDbConnection CreateConnection();
    }
}
