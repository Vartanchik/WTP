using System;
using System.Collections.Generic;
using System.Text;
using WTP.BLL.DTOs.ServicesDTOs;

namespace WTP.BLL.DTOs.TeamDTOs
{
    public class TeamIndexDto
    {
        public IEnumerable<TeamListItemDto> Teams { get; set; } // List of teams at current page
        public PageDto PageViewModel { get; set; } // data about paging

    }
}
