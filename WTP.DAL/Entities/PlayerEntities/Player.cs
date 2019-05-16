using WTP.DAL.Entities.AppUserEntities;

namespace WTP.DAL.Entities.PlayerEntities
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
        public int Rank { get; set; }
        public int Decency { get; set; }
    }
}
