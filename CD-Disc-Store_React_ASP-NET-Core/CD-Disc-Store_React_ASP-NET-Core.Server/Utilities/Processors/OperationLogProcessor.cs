using Dapper;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Processors
{
    public class OperationLogProcessor : Processor<OperationLog>
    {
        public override string GetSearchConditions(string searchText, DynamicParameters parameters) => "1=1";
    }
}
