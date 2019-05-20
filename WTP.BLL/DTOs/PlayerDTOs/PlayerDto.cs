using WTP.BLL.DTOs.AppUserDTOs;

namespace WTP.BLL.DTOs.PlayerDTOs
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AppUserDto AppUserDto { get; set; }
        public GameDto GameDto { get; set; }
        public ServerDto ServerDto { get; set; }
        public GoalDto GoalDto { get; set; }
        public string About { get; set; }
        public int Rank { get; set; }
        public int Decency { get; set; }
    }
}
