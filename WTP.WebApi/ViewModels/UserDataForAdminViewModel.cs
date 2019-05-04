using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WTP.WebAPI.ViewModels
{
    public class UserDataForAdminViewModel : AppUserDtoViewModel
    {
        public int Id { get; set; }

        public bool LockoutEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public bool DeletedStatus { get; set; }
    }
}
