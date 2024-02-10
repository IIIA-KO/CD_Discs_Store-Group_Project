using Microsoft.Data.SqlClient;
using CdDiskStoreAspNetCore.Models.Interfaces.Data;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.ViewModels
{
    public class Processable<T> : IDataProcessable where T : class
    {
        public static IReadOnlyList<string> AllFieldNames { get; private set; } =
           typeof(T).GetProperties()
           .Select(p => p.Name)
           .ToList();

        public string? SearchText { get; set; } = default!;

        public string? SortFieldName { get; set; } = default!;

        public SortOrder SortOrder { get; set; }

        public int Skip { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public int CountItems { get; set; }

        public IEnumerable<T>? Items { get; set; }
    }
}
