using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WTP.WebAPI.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Column(TypeName = "nvarchar(16)")]
        public string Password { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
