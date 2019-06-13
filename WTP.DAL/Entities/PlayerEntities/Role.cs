namespace WTP.DAL.Entities
{
    public class Role : IEntity
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string Name { get; set; }
    }
}
