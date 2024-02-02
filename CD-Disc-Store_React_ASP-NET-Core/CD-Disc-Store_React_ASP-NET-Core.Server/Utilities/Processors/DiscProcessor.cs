using Dapper;
using CD_Disc_Store_React_ASP_NET_Core.Server.ViewModels;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Processors
{
    public class DiscProcessor : ProcessableViewModelProcessor<Disc>
    {
        public override string GetSearchConditions(string searchText, DynamicParameters parameters)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return "1=1";
            }

            var conditions = new List<string>();

            foreach (var fieldName in ProcessableViewModel<Disc>.AllFieldNames)
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