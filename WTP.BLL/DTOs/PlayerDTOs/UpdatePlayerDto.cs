using System.ComponentModel.DataAnnotations;

namespace WTP.BLL.DTOs.PlayerDTOs
{
    public class UpdatePlayerDto
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
        public string About { get; set; }
        [Required]
        public int RankId { get; set; }
        [Required]
        [Range(1, 10000)]
        public int Decency { get; set; }
    }
}
