namespace WTP.DAL.Entities.TeamEntities
{
    public class Invitation : IEntity
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public Player Player {get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public Author Author { get; set; }
    }
}
