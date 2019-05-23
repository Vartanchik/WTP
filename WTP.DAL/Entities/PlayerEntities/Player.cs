using WTP.DAL.Entities.AppUserEntities;

namespace WTP.DAL.Entities
{
    public class Player : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int ServerId { get; set; }
        public Server Server { get; set; }
        public int GoalId { get; set; }
        public Goal Goal { get; set; }
        public string About { get; set; }
        public int RankId { get; set; }
        public Rank Rank { get; set; }
        public int? Decency { get; set; }
        public int? TeamId { get; set; }
        public Team Team { get; set; }
    }
}
