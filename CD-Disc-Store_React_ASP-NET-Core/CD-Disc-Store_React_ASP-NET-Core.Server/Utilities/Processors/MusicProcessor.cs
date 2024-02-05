using Dapper;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Processors
{
    public class MusicProcessor : ProcessableViewModelProcessor<Music>
    {
        public override string GetSearchConditions(string searchText, DynamicParameters parameters)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return "1=1";
            }

            var conditions = new List<string>();

            foreach (var fieldName in ProcessableViewModel<Music>.AllFieldNames)
            {
                var propertyType = typeof(Music).GetProperty(fieldName)?.PropertyType;

                if (propertyType == typeof(string))
                {
                    conditions.Add($"{fieldName} LIKE @searchText");
                    parameters.Add("@searchText", $"%{searchText}%");
                }
            }

            return string.Join(" OR ", conditions);
        }
    }
}
