using System.ComponentModel.DataAnnotations;

namespace WTP.WebAPI.Dto
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
