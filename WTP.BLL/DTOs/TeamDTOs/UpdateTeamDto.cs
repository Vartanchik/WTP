using System.ComponentModel.DataAnnotations;

namespace WTP.BLL.DTOs.TeamDTOs
{
    public class UpdateTeamDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 4)]
        public string Name { get; set; }
        [Required]
        public int ServerId { get; set; }
        [Required]
        public int GoalId { get; set; }
    }
}
