namespace WTP.BLL.DTOs.TeamDTOs
{
    public class TeamListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public int AppUser { get; set; }
        public string Game { get; set; }
        public string Server { get; set; }
        public string Goal { get; set; }
        public int WinRate { get; set; }
    }
}
