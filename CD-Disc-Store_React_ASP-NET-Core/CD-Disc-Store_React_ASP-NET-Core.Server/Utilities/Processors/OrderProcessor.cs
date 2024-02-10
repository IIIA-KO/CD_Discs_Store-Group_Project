using Dapper;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Processors
{
    public class OrderProcessor : Processor<Order>
    {
        public override string GetSearchConditions(string searchText, DynamicParameters parameters)
        {
            var conditions = new List<string>();

            foreach (var fieldName in Processable<Order>.AllFieldNames)
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
