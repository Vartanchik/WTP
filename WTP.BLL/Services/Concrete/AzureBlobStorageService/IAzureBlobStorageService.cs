using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.AppUser;

namespace WTP.BLL.Services.Concrete.AzureBlobStorageService
{
    public interface IAzureBlobStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, int userId);

        Task<FileStreamResult> DownloadFileAsync(string userPhoto);
    }
}
