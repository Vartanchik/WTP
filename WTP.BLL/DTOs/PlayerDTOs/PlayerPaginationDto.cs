using System;
using System.Collections.Generic;
using System.Text;

namespace WTP.BLL.DTOs.PlayerDTOs
{
    public class PlayerPaginationDto
    {
        public IList<PlayerListItemDto> Players { get; set; }
        public int PlayersQuantity { get; set; }
    }
}
