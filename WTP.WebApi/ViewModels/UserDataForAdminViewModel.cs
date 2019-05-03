using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WTP.WebAPI.ViewModels
{
    public class UserDataForAdminViewModel: AppUserDtoViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public bool LockoutEnabled { get; set; }
        [Required]
        public DateTimeOffset? LockoutEnd { get; set; }

    }
}
