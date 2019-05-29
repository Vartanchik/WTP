using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WTP.DAL.Entities.AppUserEntities;

namespace WTP.DAL.Entities
{
    public class Team : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public int CoachId { get; set; }
        public AppUser Coach { get; set; }
        [Range(1, 3)]
        public int GameId { get; set; }
        public Game Game { get; set; }
        [Range(1, 4)]
        public int ServerId { get; set; }
        public Server Server { get; set; }
        [Range(1, 2)]
        public int GoalId { get; set; }
        public Goal Goal { get; set; }
        [MaxLength(5)]
        public List<Player> Players { get; set; }
        [Range(0, 100)]
        public int WinRate { get; set; }
    }
}
