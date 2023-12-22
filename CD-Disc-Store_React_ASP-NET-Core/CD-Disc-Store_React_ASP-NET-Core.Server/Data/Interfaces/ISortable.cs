using Microsoft.Data.SqlClient;

namespace CdDiskStoreAspNetCore.Models.Interfaces.Data
{
    public interface ISortable
    {
        public string? SortFieldName { get; set; }
        public SortOrder SortOrder { get; set; }
    }
}