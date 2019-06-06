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
        public string AppUserName { get; set; }
        public string AppUserEmail { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; }
        public int ServerId { get; set; }
        public string ServerName { get; set; }
        public int GoalId { get; set; }
        public string GoalName { get; set; }
        public string About { get; set; }
        public int RankId { get; set; }
        public string RankName { get; set; }
        public int Decency { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
    }
}
