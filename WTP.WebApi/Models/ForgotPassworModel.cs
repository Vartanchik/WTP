using System.ComponentModel.DataAnnotations;

namespace WTP.WebAPI.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
