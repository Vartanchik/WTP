using System.ComponentModel.DataAnnotations;

namespace WTP.BLL.DTOs.ServicesDTOs
{
    public class LoginDto
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
