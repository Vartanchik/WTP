using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace WTP.BLL.Services.Concrete.AzureBlobStorageService
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        private readonly IConfiguration _configuration;

        private readonly CloudBlobContainer _cloudBlobContainer;

        public AzureBlobStorageService(IConfiguration configuration)
        {
            _configuration = configuration;

            var accountName = _configuration["AppSettings:AccountName"];

            var containerName = _configuration["AppSettings:ContainerName"];

            var accountKey = _configuration["AppSettings:AccountKey"];

            var storageCredentials = new StorageCredentials(accountName, accountKey);

            var cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);

            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            _cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
        }

        public async Task<string> UploadFileAsync(string base64StringWithHeaders, string userPhoto)
        {
            if(userPhoto == null)
            {
                userPhoto = "image" + Guid.NewGuid().ToString();
            }

            var cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference(userPhoto);

            // Remove headers from base64 string
            var base64String = base64StringWithHeaders.Substring(base64StringWithHeaders.IndexOf(',') + 1);

            var bytes = Convert.FromBase64String(base64String);

            using (var memoryStream = new MemoryStream(bytes))
            {
                await cloudBlockBlob.UploadFromStreamAsync(memoryStream);
            }

            return userPhoto;
        }

        public async Task<string> DownloadFileAsync(string userPhoto)
        {
            var cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference(userPhoto);

            using (var memoryStream = new MemoryStream())
            {
                await cloudBlockBlob.DownloadToStreamAsync(memoryStream);

                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        public async Task DeleteFileAsync(string userPhoto)
        {
            var cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference(userPhoto);

            await cloudBlockBlob.DeleteAsync();
        }
    }
}
