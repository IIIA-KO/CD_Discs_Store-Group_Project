using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Services.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Services.Implementations
{
    public class GoogleCloudStorage : ICloudStorage
    {
        private readonly GoogleCredential _googleCredential;
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;

        public GoogleCloudStorage(IConfiguration configuration)
        {
            this._googleCredential = GoogleCredential.FromFile(configuration.GetValue<string>("GoogleCredentialFile"));
            this._storageClient = StorageClient.Create(this._googleCredential);
            this._bucketName = configuration.GetValue<string>("GoogleCloudStorageBucket") ??
                throw new NullReferenceException("Unable to get Google Cloud Storage Bucket name.");
        }

        public async Task<string> UploadFileAsync(IFormFile file, string fileNameForStorage)
        {
            using(var memorySream = new MemoryStream())
            {
                await file.CopyToAsync(memorySream);
                var dataObject = await this._storageClient.UploadObjectAsync(this._bucketName, fileNameForStorage, null, memorySream);
                return dataObject.MediaLink;
            }
        }

        public async Task DeleteFileAsync(string fileNameForStorage)
        {
            await this._storageClient.DeleteObjectAsync(this._bucketName, fileNameForStorage);
        }
    }
}