using System.Collections.Generic;
using WTP.BLL.DTOs.PlayerDTOs;

namespace WTP.BLL.DTOs.TeamDTOs
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public int AppUserId { get; set; }
        public string Game { get; set; }
        public string Server { get; set; }
        public string Goal { get; set; }
        public IList<PlayerListItemOnTeamPageDto> Players { get; set; }
        public int WinRate { get; set; }
    }
}
