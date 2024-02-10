using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Interfaces;

namespace CdDiskStoreAspNetCore.Models.Interfaces.Data
{
    public interface IDataProcessable : ISearchable, ISortable, IPaginable
    {
    }
}
