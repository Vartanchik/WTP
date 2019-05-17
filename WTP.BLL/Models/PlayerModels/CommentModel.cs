using System;

namespace WTP.BLL.Models.PlayerModels
{
    public class CommentModel : IModel
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public PlayerModel Author { get; set; }
        public PlayerModel Receive { get; set; }
        public string Text { get; set; }
        public int Mark { get; set; }
    }
}
