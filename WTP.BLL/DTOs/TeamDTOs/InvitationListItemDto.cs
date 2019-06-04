using System;
using System.Collections.Generic;
using System.Text;

namespace WTP.BLL.DTOs.TeamDTOs
{
    public class InvitationListItemDto
    {
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public string TeamName { get; set; }
        public string Author { get; set; }
    }
}
