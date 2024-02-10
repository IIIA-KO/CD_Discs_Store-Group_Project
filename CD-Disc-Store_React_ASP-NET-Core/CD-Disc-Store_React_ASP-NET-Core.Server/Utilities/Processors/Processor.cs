using Dapper;
using Microsoft.Data.SqlClient;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Processors
{
    public abstract class Processor<T> where T : class
    {
        public virtual string GetSqlQuery(Processable<T> processable, DynamicParameters parameters)
        {
            if (string.IsNullOrEmpty(processable.SortFieldName)
                || !Processable<T>.AllFieldNames.Any(f => string.Equals(f, processable.SortFieldName, StringComparison.OrdinalIgnoreCase)))
            {
                return $"SELECT * FROM {typeof(T).Name}";
            }

            var sortOrder = processable.SortOrder == SortOrder.Descending ? "DESC" : "ASC";

            var conditions = processable.SearchText == null
                ? "1=1"
                : GetSearchConditions(processable.SearchText, parameters);

            return $"SELECT * FROM {typeof(T).Name} WHERE ({conditions}) ORDER BY {processable.SortFieldName} {sortOrder} OFFSET {processable.Skip} ROWS FETCH NEXT {processable.PageSize} ROWS ONLY";
        }

        public virtual string GetCountQuery(string? searchText, DynamicParameters parameters)
        {
            var conditions = searchText == null
                ? "1=1"
                : GetSearchConditions(searchText, parameters);

            return $"SELECT COUNT(*) FROM {typeof(T).Name} WHERE ({conditions})";
        }

        public abstract string GetSearchConditions(string searchText, DynamicParameters parameters);
    }
}
