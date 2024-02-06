using Dapper;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Processors
{
    public class FilmProcessor : Processor<Film>
    {
        public override string GetSearchConditions(string searchText, DynamicParameters parameters)
        {
            var conditions = new List<string>();

            foreach (var fieldName in Processable<Film>.AllFieldNames)
            {
                var propertyType = typeof(Film).GetProperty(fieldName)?.PropertyType;

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
            }

            return string.Join(" OR ", conditions);
        }
    }
}
