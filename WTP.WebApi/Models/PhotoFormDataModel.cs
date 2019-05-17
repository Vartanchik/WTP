using Microsoft.AspNetCore.Http;

namespace WTP.WebAPI.Models
{
    public class PhotoFormDataModel
    {
        public IFormFile File { get; set; }
    }
}
