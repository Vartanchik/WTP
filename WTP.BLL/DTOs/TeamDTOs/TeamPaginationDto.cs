using System;
using System.Collections.Generic;
using System.Text;

namespace WTP.BLL.DTOs.TeamDTOs
{
    public class TeamPaginationDto
    {
        public IList<TeamListItemDto> Teams { get; set; }
        public int TeamsQuantity { get; set; }

    }
}
