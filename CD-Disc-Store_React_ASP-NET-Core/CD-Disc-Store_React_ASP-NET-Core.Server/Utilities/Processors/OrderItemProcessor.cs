using Dapper;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Processors
{
    public class OrderItemProcessor : Processor<OrderItem>
    {
        public override string GetSearchConditions(string searchText, DynamicParameters parameters)
        {
            var conditions = new List<string>();

            foreach (var fieldName in Processable<Order>.AllFieldNames)
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
