using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace WTP.WebAPI.Utility.Extensions
{
    public class BlobStorageMultipartStreamProvider //: MultipartFormDataStreamProvider
    {
        private readonly IConfiguration _configuration;


        public BlobStorageMultipartStreamProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task UploadFileAsync(string path, string fileName)
        {
            var cloudBlockBlob = GetBlobContainer().GetBlockBlobReference(fileName);

            using (var fileStream = File.OpenRead(path))
            {
                await cloudBlockBlob.UploadFromStreamAsync(fileStream);
            }
        }
        public async Task UploadFileAsync(Stream stream, string fileName)
        {
            var cloudBlockBlob = GetBlobContainer().GetBlockBlobReference(fileName);

            await cloudBlockBlob.UploadFromStreamAsync(stream);
        }
        public async Task DownloadFileAsync(string path, string fileName)
        {
            var cloudBlockBlob = GetBlobContainer().GetBlockBlobReference(fileName);

            using (var fileStream = File.OpenWrite(path))
            {
                await cloudBlockBlob.DownloadToStreamAsync(fileStream);
            }
        }

        private CloudBlobContainer GetBlobContainer()
        {
            string accountName = _configuration["AppSettings:AccountName"];

            string containerName = _configuration["AppSettings:ContainerName"];

            string accountKey = _configuration["AppSettings:AccountKey"];

            var storageConnection = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accountKey);

            var cloudStorageAccount = new CloudStorageAccount(storageConnection, true);

            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            return cloudBlobClient.GetContainerReference(containerName);
        }
    }
}
