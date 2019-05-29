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
        public AppUserDto Coach { get; set; }
        public GameDto Game { get; set; }
        public ServerDto Server { get; set; }
        public GoalDto Goal { get; set; }
        [MaxLength(5)]
        public List<PlayerDto> Players { get; set; }
        [Range(0, 100)]
        public int WinRate { get; set; }
    }
}
