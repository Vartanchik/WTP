using Microsoft.AspNetCore.Http;

namespace WTP.WebAPI.ViewModels
{
    public class PhotoFormDataModel
    {
        public IFormFile File { get; set; }
    }
}
