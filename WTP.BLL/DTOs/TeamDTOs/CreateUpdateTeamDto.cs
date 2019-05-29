using System.ComponentModel.DataAnnotations;

namespace WTP.BLL.DTOs.TeamDTOs
{
    public class CreateUpdateTeamDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1, 3)]
        public int GameId { get; set; }
        [Required]
        [Range(1, 4)]
        public int ServerId { get; set; }
        [Required]
        [Range(1, 2)]
        public int GoalId { get; set; }
    }
}
