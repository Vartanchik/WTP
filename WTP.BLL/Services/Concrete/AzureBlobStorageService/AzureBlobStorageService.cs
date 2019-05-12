using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;
using WTP.Logging;

namespace WTP.BLL.Services.Concrete.AzureBlobStorageService
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        private readonly IConfiguration _configuration;

        private readonly ILog _log;

        private readonly CloudBlobContainer _cloudBlobContainer;

        public AzureBlobStorageService(IConfiguration configuration, ILog log)
        {
            _configuration = configuration;

            _log = log;

            var accountName = _configuration["AppSettings:AccountName"];

            var containerName = _configuration["AppSettings:ContainerName"];

            var accountKey = _configuration["AppSettings:AccountKey"];

            var storageCredentials = new StorageCredentials(accountName, accountKey);

            var cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);

            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            _cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
        }

        public async Task<string> UploadFileAsync(IFormFile file, string userPhoto)
        {
            CloudBlockBlob cloudBlockBlob;

            if (userPhoto == null)
            {
                userPhoto = "image" + Guid.NewGuid().ToString();
            }

            cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference(userPhoto);

            if (await cloudBlockBlob.ExistsAsync())
            {
                await cloudBlockBlob.DeleteAsync();
            }

            cloudBlockBlob.Properties.ContentType = file.ContentType;

            await cloudBlockBlob.UploadFromStreamAsync(file.OpenReadStream());

            return userPhoto;
        }

        public async Task<FileStreamResult> DownloadFileAsync(string userPhoto)
        {
            var cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference(userPhoto);

            var memoryStream = new MemoryStream();

            await cloudBlockBlob.DownloadToStreamAsync(memoryStream);

            memoryStream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(memoryStream, cloudBlockBlob.Properties.ContentType);
        }
    }
}
