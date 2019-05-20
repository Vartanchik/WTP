namespace WTP.BLL.DTOs.PlayerDTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public PlayerDto Author { get; set; }
        public PlayerDto Receive { get; set; }
        public string Text { get; set; }
        public int Mark { get; set; }
    }
}
