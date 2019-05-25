using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.DTOs.TeamDTOs;

namespace WTP.BLL.DTOs.PlayerDTOs
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AppUserDto AppUser { get; set; }
        public GameDto Game { get; set; }
        public ServerDto Server { get; set; }
        public GoalDto Goal { get; set; }
        public string About { get; set; }
        public int RankId { get; set; }
        public RankDto Rank { get; set; }
        public int Decency { get; set; }
        public TeamDto Team { get; set; }
    }
}
