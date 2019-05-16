using System.Threading.Tasks;
using WTP.BLL.Models.Azure;

namespace WTP.BLL.Services.Concrete.AzureBlobStorageService
{
    public interface IAzureBlobStorageService
    {
        Task<string> UploadFileAsync(FileDataModel file, AzureBlobStorageConfigModel configuration);

        Task<FileDataModel> DownloadFileAsync(string blockBlobNamge, AzureBlobStorageConfigModel configuration);
    }
}
