using Dapper;
using CD_Disc_Store_React_ASP_NET_Core.Server.ViewModels;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Processors
{
    public class ClientProcessor : ProcessableViewModelProcessor<Client>
    {
        public override string GetSearchConditions(string searchText, DynamicParameters parameters)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return "1=1";
            }

            var conditions = new List<string>();

            foreach (var fieldName in ProcessableViewModel<Client>.AllFieldNames)
            {
                var propertyType = typeof(Client).GetProperty(fieldName)?.PropertyType;

                if (propertyType == typeof(string))
                {
                    conditions.Add($"{fieldName} LIKE @searchText");
                    parameters.Add("@searchText", $"%{searchText}%");
                }
                else if (propertyType == typeof(DateTime) && DateTime.TryParse(searchText, out var parsedDate))
                {
                    conditions.Add($"{fieldName} = @searchDate");
                    parameters.Add("@searchDate", parsedDate);
                }
                else if (propertyType == typeof(bool) && bool.TryParse(searchText, out var parsedBool))
                {
                    conditions.Add($"{fieldName} = @searchBool");
                    parameters.Add("@searchBool", parsedBool);
                }
            }

            return string.Join(" OR ", conditions);
        }
    }
}
