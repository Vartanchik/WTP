using System.Threading.Tasks;
using WTP.BLL.ModelsDto.Azure;

namespace WTP.BLL.Services.Concrete.AzureBlobStorageService
{
    public interface IAzureBlobStorageService
    {
        Task<string> UploadFileAsync(FileDataDto file, AzureBlobStorageConfigModel configuration);

        Task<FileDataDto> DownloadFileAsync(string blockBlobNamge, AzureBlobStorageConfigModel configuration);
    }
}
