using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.History;

namespace WTP.WebAPI.ViewModels
{
    public class HistoryIndexViewModel
    {
        public IEnumerable<HistoryDto> Histories { get; set; } // List of users at current page
        public PageViewModel PageViewModel { get; set; } // data about paging
        public HistoryFilterViewModel FilterViewModel { get; set; } // data about filters
        public HistorySortViewModel SortViewModel { get; set; } // data about sorting
    }
}
