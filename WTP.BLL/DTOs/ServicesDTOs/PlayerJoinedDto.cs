using System;
using System.Collections.Generic;
using System.Text;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.TeamDTOs;
using WTP.DAL.Entities;

namespace WTP.BLL.DTOs.ServicesDTOs
{
    public class PlayerJoinedDto
    {
        //public Player Player { get; set; }
        //public Game Game { get; set; }
        //public AppUser AppUser { get; set; }
        //public Server Server { get; set; }
        //public Team Team { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public ShortUserFormDto AppUser { get; set; }
        public GameDto Game { get; set; }
        public ServerDto Server { get; set; }
        public GoalDto Goal { get; set; }
        public string About { get; set; }
        public RankDto Rank{get;set;}
        public int Decency { get; set; }
        public TeamDto Team { get; set; }
    }
}
