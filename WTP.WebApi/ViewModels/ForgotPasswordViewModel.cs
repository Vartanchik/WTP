using System.ComponentModel.DataAnnotations;

namespace WTP.WebAPI.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
