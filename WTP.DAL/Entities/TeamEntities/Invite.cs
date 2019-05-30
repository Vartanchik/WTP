namespace WTP.DAL.Entities.TeamEntities
{
    public class Invite : IEntity
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public Player Player {get; set;}
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public bool Invitation { get; set; }
    }
}
