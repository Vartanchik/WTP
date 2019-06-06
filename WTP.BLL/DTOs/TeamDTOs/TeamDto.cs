using System.ComponentModel.DataAnnotations;

namespace WTP.BLL.DTOs.TeamDTOs
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public int AppUserId { get; set; }
        public string Game { get; set; }
        public string Server { get; set; }
        public string Goal { get; set; }
        [Range(1, 100)]
        public int WinRate { get; set; }
    }
}
