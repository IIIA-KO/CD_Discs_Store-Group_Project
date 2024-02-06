using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Interfaces;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Services
{
    public interface ICloudStorage
    {
        Task<bool> UploadFileAsync(IImage entity);

        Task DeleteFileAsync(string fileNameForStorage);

        string FormFileName(string title, string fileName);
    }
}
