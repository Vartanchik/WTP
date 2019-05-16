using System;

namespace WTP.DAL.Entities.PlayerEntities
{
    public class Comment : IEntity
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int AuthorId { get; set; }
        public Player Author { get; set; }
        public int ReceiveId { get; set; }
        public Player Receive { get; set; }
        public string Text { get; set; }
        public int Mark { get; set; }
    }
}
