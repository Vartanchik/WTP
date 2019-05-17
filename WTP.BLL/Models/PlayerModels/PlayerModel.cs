using WTP.BLL.Models.AppUserModels;

namespace WTP.BLL.Models.PlayerModels
{
    public class PlayerModel : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AppUserModel AppUserModel { get; set; }
        public GameModel GameModel { get; set; }
        public ServerModel ServerModel { get; set; }
        public GoalModel GoalModel { get; set; }
        public string About { get; set; }
        public int Rank { get; set; }
        public int Decency { get; set; }
    }
}
