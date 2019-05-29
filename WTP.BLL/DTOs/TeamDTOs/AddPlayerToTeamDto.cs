using System.ComponentModel.DataAnnotations;

namespace WTP.BLL.DTOs.TeamDTOs
{
    public class AddPlayerToTeamDto
    {
        [Required]
        [Range(1, 3)]
        public int GameId { get; set; }
        [Required]
        public int PlayerId { get; set; }
    }
}
