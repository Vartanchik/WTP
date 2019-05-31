namespace WTP.DAL.Entities.TeamEntities
{
    public class Invitations : IEntity
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public Player Player {get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public Invitation Author { get; set; }
    }

    public enum Invitation
    {
        Coach,
        Player
    }
}
