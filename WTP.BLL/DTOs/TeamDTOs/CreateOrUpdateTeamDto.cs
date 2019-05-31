using System.ComponentModel.DataAnnotations;

namespace WTP.BLL.DTOs.TeamDTOs
{
    public class CreateOrUpdateTeamDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int GameId { get; set; }
        [Required]
        public int ServerId { get; set; }
        [Required]
        public int GoalId { get; set; }
    }
}
