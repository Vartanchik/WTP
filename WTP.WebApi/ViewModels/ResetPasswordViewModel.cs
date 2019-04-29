using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WTP.WebAPI.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Column(TypeName = "nvarchar(16)")]
        public string Password { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
