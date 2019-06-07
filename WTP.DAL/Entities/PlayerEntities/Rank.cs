namespace WTP.DAL.Entities
{
    public class Rank : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Value { get; set; }
    }
}
