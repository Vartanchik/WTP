using System.ComponentModel.DataAnnotations;

namespace WTP.BLL.DTOs.TeamDTOs
{
    public class CreateTeamDto
    {
        [Required]
        [StringLength(16, MinimumLength = 4)]
        public string Name { get; set; }
        [Required]
        public int GameId { get; set; }
        [Required]
        public int ServerId { get; set; }
        [Required]
        public int GoalId { get; set; }
    }
}
