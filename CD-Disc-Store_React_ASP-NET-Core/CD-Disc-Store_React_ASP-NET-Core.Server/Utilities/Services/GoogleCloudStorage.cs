using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Interfaces;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Atributes;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Services
{
    public class GoogleCloudStorage : ICloudStorage
    {
        private readonly GoogleCredential _googleCredential;
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;

        public GoogleCloudStorage(IConfiguration configuration)
        {
            _googleCredential = GoogleCredential.FromFile(configuration.GetValue<string>("GoogleCredentialFile"));
            _storageClient = StorageClient.Create(_googleCredential);
            _bucketName = configuration.GetValue<string>("GoogleCloudStorageBucket")
                ?? throw new NullReferenceException("Unable to get Google Cloud Storage Bucket name.");
        }

        public async Task<bool> UploadFileAsync(IImage entity)
        {
            if (entity == null || entity.ImageFile == null)
            {
                return false;
            }

            if (!IsValidImage(entity.ImageFile))
            {
                return false;
            }

            using var memorySream = new MemoryStream();
            var fileNameForStorage = FormFileName(entity.Name, entity.ImageFile.FileName);

            await entity.ImageFile.CopyToAsync(memorySream);
            var dataObject = await _storageClient.UploadObjectAsync(_bucketName, fileNameForStorage, null, memorySream);

            entity.CoverImagePath = dataObject.MediaLink;
            entity.ImageStorageName = fileNameForStorage;

            return true;
        }

        private bool IsValidImage(IFormFile imageFile)
        {
            if (imageFile.Length > 1 * 1024 * 1024)
            {
                return false;
            }

            var extension = Path.GetExtension(imageFile.FileName).ToLower();

            if (!AllowedImageExtensions.AllowedExtensions.Contains(extension))
            {
                return false;
            }

            return true;
        }

        public string FormFileName(string name, string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var fileNameForStorage = $"{name}-{Guid.NewGuid()}{fileExtension}";
            return fileNameForStorage;
        }

        public async Task DeleteFileAsync(string fileNameForStorage)
        {
            await _storageClient.DeleteObjectAsync(_bucketName, fileNameForStorage);
        }
    }
}
