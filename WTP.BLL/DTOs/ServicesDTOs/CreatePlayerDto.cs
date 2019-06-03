using System.ComponentModel.DataAnnotations;

namespace WTP.BLL.DTOs.ServicesDTOs
{
    public class CreateUpdatePlayerDto
    {
        [Required]
        [StringLength(16, MinimumLength = 4)]
        public string Name { get; set; }
        public int GameId { get; set; }
        public int ServerId { get; set; }
        public int GoalId { get; set; }
        public string About { get; set; }
        public int RankId { get; set; }
        [Range(1, 10000)]
        public int Decency { get; set; }
    }
}
