using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WTP.BLL.Services.Concrete.AzureBlobStorageService
{
    public interface IAzureBlobStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string userPhoto);

        Task<FileStreamResult> DownloadFileAsync(string userPhoto);
    }
}
