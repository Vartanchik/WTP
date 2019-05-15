using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WTP.WebAPI.Dto
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "User Name")]
        [Column(TypeName = "nvarchar(30)")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Column(TypeName = "nvarchar(16)")]
        public string Password { get; set; }
    }
}
