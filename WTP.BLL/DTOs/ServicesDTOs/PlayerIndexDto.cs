using System.Collections.Generic;
using WTP.BLL.DTOs.PlayerDTOs;

namespace WTP.BLL.DTOs.ServicesDTOs
{
    public class PlayerIndexDto
    {
        public IEnumerable<PlayerListItemDto> Players { get; set; } // List of players at current page
        public PageDto PageViewModel { get; set; } // data about paging

    }
}
