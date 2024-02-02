using Dapper;
using CD_Disc_Store_React_ASP_NET_Core.Server.ViewModels;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Processors
{
    public class OrderItemProcessor : ProcessableViewModelProcessor<OrderItem>
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

                if (propertyType == typeof(int) && int.TryParse(searchText, out var parsedInt))
                {
                    conditions.Add($"{fieldName} = @searchInt");
                    parameters.Add("@searchInt", parsedInt);
                }
            }

            return string.Join(" OR ", conditions);
        }
    }
}
