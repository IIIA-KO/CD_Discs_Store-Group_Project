using Dapper;
using CD_Disc_Store_React_ASP_NET_Core.Server.ViewModels;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Processors
{
    public class OrderProcessor : ProcessableViewModelProcessor<Order>
    {
        public override string GetSearchConditions(string searchText, DynamicParameters parameters)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return "1=1";
            }

            var conditions = new List<string>();

            foreach (var fieldName in ProcessableViewModel<Order>.AllFieldNames)
            {
                var propertyType = typeof(Order).GetProperty(fieldName)?.PropertyType;

                if (propertyType == typeof(DateTime) && DateTime.TryParse(searchText, out var parsedDate))
                {
                    conditions.Add($"{fieldName} = @searchDate");
                    parameters.Add("@searchDate", parsedDate);
                }
            }

            return string.Join(" OR ", conditions);
        }
    }
}
