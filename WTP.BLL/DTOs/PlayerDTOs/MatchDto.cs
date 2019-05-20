namespace WTP.BLL.DTOs.PlayerDTOs
{
    public class MatchDto
    {
        public int Id { get; set; }
        public GameDto Game { get; set; }
        public string Date { get; set; }
        public PlayerDto Player { get; set; }
        public bool Win { get; set; }
    }
}
