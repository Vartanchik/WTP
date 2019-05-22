using System;
using System.Collections.Generic;
using System.Text;
using WTP.BLL.DTOs.AppUserDTOs;

namespace WTP.BLL.DTOs.ServicesDTOs
{
    public class HistoryIndexDto
    {
        public IEnumerable<HistoryDto> Histories { get; set; } // List of users at current page
        public PageDto PageViewModel { get; set; } // data about paging
        public HistoryFilterDto FilterViewModel { get; set; } // data about filters
        public HistorySortDto SortViewModel { get; set; } // data about sorting
    }
}
