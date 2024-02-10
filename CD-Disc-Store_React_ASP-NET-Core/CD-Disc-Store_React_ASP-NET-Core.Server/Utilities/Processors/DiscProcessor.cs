using Dapper;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Processors
{
    public class DiscProcessor : Processor<Disc>
    {
        public override string GetSearchConditions(string searchText, DynamicParameters parameters)
        {
            var conditions = new List<string>();

            foreach (var fieldName in Processable<Disc>.AllFieldNames)
            {
                var propertyType = typeof(Disc).GetProperty(fieldName)?.PropertyType;

                if (propertyType == typeof(string))
                {
                    conditions.Add($"{fieldName} LIKE @searchText");
                    parameters.Add("@searchText", $"%{searchText}%");
                }
                else if (propertyType == typeof(int) && int.TryParse(searchText, out var parsedInt))
                {
                    conditions.Add($"{fieldName} = @searchInt");
                    parameters.Add("@searchInt", parsedInt);
                }
                else if (propertyType == typeof(decimal) && int.TryParse(searchText, out var parsedDecimal))
                {
                    conditions.Add($"{fieldName} = @searchDecimal");
                    parameters.Add("@searchDecimal", parsedDecimal);
                }
            }

            return string.Join(" OR ", conditions);
        }
    }
}
