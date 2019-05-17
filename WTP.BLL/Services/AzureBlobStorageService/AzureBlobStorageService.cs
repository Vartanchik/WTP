using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;
using WTP.BLL.Models.Azure;
using WTP.Logging;

namespace WTP.BLL.Services.AzureBlobStorageService
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
            try
            {
                var blockBlobNamge = file.BlobName;

                if (blockBlobNamge == null)
                {
                    blockBlobNamge = configuration.BlobStorageUrl + Guid.NewGuid().ToString();
                }

                var cloudBlobContainer = GetCloudBlobContainer(configuration);

                if (cloudBlobContainer == null)
                {
                    return null;
                }

                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blockBlobNamge);

                await cloudBlockBlob.DeleteIfExistsAsync();

                cloudBlockBlob.Metadata.Add("Name", Base64Encode(file.Name));

                cloudBlockBlob.Properties.ContentType = file.Type;

                await cloudBlockBlob.UploadFromStreamAsync(file.Stream);

                return blockBlobNamge;
            }
            catch (Exception ex)
            {
                _log.Error($"UploadFileAsync error message:{ex.Message}");

                return null;
            }
        }

        public async Task<FileDataModel> DownloadFileAsync(string blockBlobNamge, AzureBlobStorageConfigModel configuration)
        {
            try
            {
                var cloudBlobContainer = GetCloudBlobContainer(configuration);

                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blockBlobNamge);

                if (!await cloudBlockBlob.ExistsAsync())
                {
                    return null;
                }

                var memoryStream = new MemoryStream();

                await cloudBlockBlob.DownloadToStreamAsync(memoryStream);

                memoryStream.Seek(0, SeekOrigin.Begin);

                return new FileDataModel(memoryStream, cloudBlockBlob.Properties.ContentType, Base64Decode(cloudBlockBlob.Metadata["Name"]));
            }
            catch (Exception ex)
            {
                _log.Error($"DownloadFileAsync error message:{ex.Message}");

                return null;
            }
        }

        private CloudBlobContainer GetCloudBlobContainer(AzureBlobStorageConfigModel configuration)
        {
            try
            {
                var accountName = configuration.AccountName;

                var accountKey = configuration.AccountKey;

                var containerName = configuration.ContainerName;

                var storageCredentials = new StorageCredentials(accountName, accountKey);

                var cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);

                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

                return cloudBlobClient.GetContainerReference(containerName);
            }
            catch (Exception ex)
            {
                _log.Error($"GetCloudBlobContainer error message:{ex.Message}");

                return null;
            }
        }

        private string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);

            return System.Convert.ToBase64String(plainTextBytes);
        }

        private string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);

            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
