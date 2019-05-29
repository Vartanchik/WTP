using System;
using System.Collections.Generic;
using System.Text;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.DTOs.TeamDTOs;

namespace WTP.BLL.DTOs.PlayerDTOs
{
    public class PlayerShortDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AppUserId { get; set; }
        public ShortUserFormDto AppUser { get; set; }
        public int GameId { get; set; }
        public GameDto Game { get; set; }
        public int ServerId { get; set; }
        public ServerDto Server { get; set; }
        public int GoalId { get; set; }
        public GoalDto Goal { get; set; }
        public string About { get; set; }
        public int RankId { get; set; }
        public RankDto Rank { get; set; }
        public int Decency { get; set; }
        public int TeamId { get; set; }
        public TeamDto Team { get; set; }
    }
}
