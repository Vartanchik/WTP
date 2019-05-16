using Microsoft.AspNetCore.Http;

namespace WTP.WebAPI.Dto
{
    public class PhotoFormDataDto
    {
        public IFormFile File { get; set; }
    }
}
