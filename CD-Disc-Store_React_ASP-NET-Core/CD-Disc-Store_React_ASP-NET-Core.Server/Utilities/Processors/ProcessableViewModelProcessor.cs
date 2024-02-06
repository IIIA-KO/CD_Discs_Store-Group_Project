using Dapper;
using Microsoft.Data.SqlClient;
using CD_Disc_Store_React_ASP_NET_Core.Server.ViewModels;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Processors
{
    public abstract class ProcessableViewModelProcessor<T> where T : class
    {
        public virtual string GetSqlQuery(ProcessableViewModel<T> processable, DynamicParameters parameters)
        {
            if (string.IsNullOrEmpty(processable.SortFieldName)
                || !ProcessableViewModel<T>.AllFieldNames.Any(f => string.Equals(f, processable.SortFieldName, StringComparison.OrdinalIgnoreCase)))
            {
                return "SELECT * FROM Client";
            }

            var sortOrder = processable.SortOrder == SortOrder.Descending ? "DESC" : "ASC";
            var conditions = GetSearchConditions(processable.SearchText, parameters);
            return $"SELECT * FROM {typeof(T).Name} WHERE ({conditions}) ORDER BY {processable.SortFieldName} {sortOrder} OFFSET {processable.Skip} ROWS FETCH NEXT {processable.PageSize} ROWS ONLY";
        }

        public abstract string GetSearchConditions(string searchText, DynamicParameters parameters);
    }
}
