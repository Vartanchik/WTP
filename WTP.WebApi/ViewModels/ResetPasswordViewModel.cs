using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WTP.WebAPI.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Column(TypeName = "nvarchar(16)")]
        public string Password { get; set; }

        public string Code { get; set; }
    }
}
