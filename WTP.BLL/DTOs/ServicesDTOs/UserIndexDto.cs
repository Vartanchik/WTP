using System;
using System.Collections.Generic;
using System.Text;
using WTP.BLL.DTOs.AppUserDTOs;

namespace WTP.BLL.DTOs.ServicesDTOs
{
    public class UserIndexDto
    {
        public IEnumerable<AppUserDto> Users { get; set; } // List of users at current page
        public PageDto PageViewModel { get; set; } // data about paging
        public UserFilterDto FilterViewModel { get; set; } // data about filters
        public UserSortDto SortViewModel { get; set; } // data about sorting

    }
}
