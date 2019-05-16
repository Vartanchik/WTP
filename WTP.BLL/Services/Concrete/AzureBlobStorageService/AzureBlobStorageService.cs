using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;
using WTP.BLL.Models.Azure;
using WTP.Logging;

namespace WTP.BLL.Services.Concrete.AzureBlobStorageService
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        private readonly ILog _log;

        public AzureBlobStorageService(ILog log)
        {
            _log = log;
        }

        public async Task<string> UploadFileAsync(FileDataModel file, AzureBlobStorageConfigModel configuration)
        {
            var cloudBlobContainer = GetCloudBlobContainer(configuration);

            var blockBlobNamge = configuration.BlobStorageUrl + Guid.NewGuid().ToString();

            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blockBlobNamge);

            await cloudBlockBlob.DeleteIfExistsAsync();

            cloudBlockBlob.Metadata.Add("Name", file.Name);

            cloudBlockBlob.Properties.ContentType = file.Type;

            await cloudBlockBlob.UploadFromStreamAsync(file.Stream);

            return blockBlobNamge;
        }

        public async Task<FileDataModel> DownloadFileAsync(string blockBlobNamge, AzureBlobStorageConfigModel configuration)
        {
            var cloudBlobContainer = GetCloudBlobContainer(configuration);

            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blockBlobNamge);

            var memoryStream = new MemoryStream();

            await cloudBlockBlob.DownloadToStreamAsync(memoryStream);

            memoryStream.Seek(0, SeekOrigin.Begin);

            return new FileDataModel(memoryStream, cloudBlockBlob.Properties.ContentType, cloudBlockBlob.Metadata["Name"]);
        }

        private CloudBlobContainer GetCloudBlobContainer(AzureBlobStorageConfigModel configuration)
        {
            var accountName = configuration.AccountName;

            var accountKey = configuration.AccountKey;

            var containerName = configuration.ContainerName;

            var storageCredentials = new StorageCredentials(accountName, accountKey);

            var cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);

            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            return cloudBlobClient.GetContainerReference(containerName);
        }
    }
}
