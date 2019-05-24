using System.ComponentModel.DataAnnotations;

namespace WTP.BLL.DTOs.ServicesDTOs
{
    public class CreateUpdatePlayerDto
    {
        [Required]
        [StringLength(16, MinimumLength = 4)]
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
        public string About { get; set; }
        [Required]
        [Range(1, 8)]
        public int RankId { get; set; }
        [Required]
        [Range(1, 10000)]
        public int Decency { get; set; }
    }
}
