using Microsoft.AspNetCore.Http;

namespace WTP.BLL.DTOs.ServicesDTOs
{
    public class PhotoFormDataDto
    {
        public IFormFile File { get; set; }
    }
}
