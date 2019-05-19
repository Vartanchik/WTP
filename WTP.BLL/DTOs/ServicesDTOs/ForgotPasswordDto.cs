using System.ComponentModel.DataAnnotations;

namespace WTP.BLL.DTOs.ServicesDTOs
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
