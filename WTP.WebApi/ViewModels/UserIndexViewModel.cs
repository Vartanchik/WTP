using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.AppUser;

namespace WTP.WebAPI.ViewModels
{
    public class UserIndexViewModel
    {
        public IEnumerable<AppUserDto> Users { get; set; } // List of users at current page
        public PageViewModel PageViewModel { get; set; } // data about paging
        public UserFilterViewModel FilterViewModel { get; set; } // data about filters
        public UserSortViewModel SortViewModel { get; set; } // data about sorting
    }
}
