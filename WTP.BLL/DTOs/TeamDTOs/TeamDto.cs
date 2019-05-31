using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.PlayerDTOs;

namespace WTP.BLL.DTOs.TeamDTOs
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public int CoachId { get; set; }
        public string Game { get; set; }
        public string Server { get; set; }
        public string Goal { get; set; }
        public List<PlayerListItemDto> Players { get; set; }
        [Range(1, 100)]
        public int WinRate { get; set; }
    }
}
