using System;

namespace WTP.DAL.Entities
{
    public class Match : IEntity
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public DateTime? Date { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public bool Win { get; set; }
    }
}
