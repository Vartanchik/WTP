using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;
using WTP.BLL.Services.Concrete.AppUserService;
using WTP.Logging;

namespace WTP.BLL.Services.Concrete.AzureBlobStorageService
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        private readonly IConfiguration _configuration;

        private readonly ILog _log;

        private readonly CloudBlobContainer _cloudBlobContainer;

        private readonly IAppUserService _appUserService;

        public AzureBlobStorageService(IConfiguration configuration, ILog log, IAppUserService appUserService)
        {
            _configuration = configuration;

            _log = log;

            _appUserService = appUserService;

            var accountName = _configuration["AppSettings:AccountName"];

            var containerName = _configuration["AppSettings:ContainerName"];

            var accountKey = _configuration["AppSettings:AccountKey"];

            var storageCredentials = new StorageCredentials(accountName, accountKey);

            var cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);

            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            _cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
        }

        public async Task<string> UploadFileAsync(IFormFile file, int userId)
        {
            var user = await _appUserService.GetAsync(userId);

            if (user.Photo == null || user.Photo == _configuration["Photo:DefaultPhoto"])
            {
                user.Photo = _configuration["Url:ImageStorageUrl"] + userId.ToString();

                var result = await _appUserService.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return null;
                }
            }

            CloudBlockBlob cloudBlockBlob;

            cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference(user.Photo);

            if (await cloudBlockBlob.ExistsAsync())
            {
                await cloudBlockBlob.DeleteAsync();
            }

            cloudBlockBlob.Properties.ContentType = file.ContentType;

            await cloudBlockBlob.UploadFromStreamAsync(file.OpenReadStream());

            return user.Photo;
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
