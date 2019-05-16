namespace WTP.DAL.Entities.PlayerEntities
{
    public class Role : IEntity
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
    }
}
