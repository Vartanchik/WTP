using System;
using System.Collections.Generic;
using System.Text;
using WTP.BLL.DTOs.ServicesDTOs;

namespace WTP.BLL.DTOs.PlayerDTOs
{
    public class PlayerManageDto
    {
        public IEnumerable<PlayerShortDto> Players { get; set; } // List of users at current page
        public PageDto PageViewModel { get; set; } // data about paging
        public PlayerManageFilterDto FilterViewModel { get; set; } // data about filters
        public PlayerManageSortDto SortViewModel { get; set; } // data about sorting
    }
}
