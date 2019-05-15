using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WTP.WebAPI.Dto
{
    public class ChangePasswordDto
    {
        [Required]
        [DataType(DataType.Password)]
        [Column(TypeName = "nvarchar(16)")]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Column(TypeName = "nvarchar(16)")]
        public string NewPassword { get; set; }
    }
}
