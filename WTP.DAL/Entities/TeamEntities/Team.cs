﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WTP.DAL.Entities.AppUserEntities;

namespace WTP.DAL.Entities.TeamEntities
{
    public class Team : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Photo { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int ServerId { get; set; }
        public Server Server { get; set; }
        public int GoalId { get; set; }
        public Goal Goal { get; set; }
        public List<Player> Players { get; set; }
        public int WinRate { get; set; }
        public ICollection<Invitation> Invitations { get; set; }
    }
}
