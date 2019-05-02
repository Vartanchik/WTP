using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WTP.WebAPI.ViewModels
{
    public class LockViewModel
    {
        [Required]       
        public int Days { get; set; }
    }
}
