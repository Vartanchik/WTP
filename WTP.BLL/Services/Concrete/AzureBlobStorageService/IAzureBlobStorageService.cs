using System.IO;
using System.Threading.Tasks;

namespace WTP.BLL.Services.Concrete.AzureBlobStorageService
{
    public interface IAzureBlobStorageService
    {
        Task<string> UploadFileAsync(string base64StringWithHeaders);

        Task<string> DownloadFileAsync(string userPhoto);

        Task DeleteFileAsync(string userPhoto);
    }
}
