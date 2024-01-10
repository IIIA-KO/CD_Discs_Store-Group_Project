namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Services.Interfaces
{
    public interface ICloudStorage
    {
        Task<string> UploadFileAsync(IFormFile file, string fileNameForStorage);

        Task DeleteFileAsync(string fileNameForStorage);
    }
}