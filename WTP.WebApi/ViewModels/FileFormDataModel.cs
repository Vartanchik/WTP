using Microsoft.AspNetCore.Http;

namespace WTP.WebAPI.ViewModels
{
    public class FileFormDataModel
    {
        public IFormFile File { get; set; }
    }
}
