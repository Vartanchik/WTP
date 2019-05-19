﻿using System.Threading.Tasks;
using WTP.BLL.DTOs.AzureDTOs;

namespace WTP.BLL.Services.AzureBlobStorageService
{
    public interface IAzureBlobStorageService
    {
        Task<string> UploadFileAsync(FileDataDto file, AzureBlobStorageConfigDto configuration);

        Task<FileDataDto> DownloadFileAsync(string blockBlobNamge, AzureBlobStorageConfigDto configuration);

        Task<bool> DeleteFileIfExistsAsync(string blockBlobNamge, AzureBlobStorageConfigDto configuration);
    }
}
