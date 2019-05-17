using System;
using System.Threading.Tasks;
using WTP.BLL.Models;
using WTP.BLL.Models.AppUserModels;
using WTP.BLL.Models.PlayerModels;
using WTP.BLL.Models.TeamModels;

namespace WTP.BLL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<AppUserModel> AppUserModels { get; }
        IRepository<RefreshTokenModel> TokenModels { get; }
        IRepository<CountryModel> CountrieModels { get; }
        IRepository<GenderModel> GenderModels { get; }
        IRepository<LanguageModel> LanguageModels { get; }
        IRepository<PlayerModel> PlayerModels { get; }
        IRepository<ServerModel> Servesr { get; }
        IRepository<GoalModel> GoalModels { get; }
        IRepository<AppUserRoleModel> AppUserRoleModels { get; }
        IRepository<PlayerRoleModel> PlayerRoleModels { get; }
        IRepository<RankModel> RankModels { get; }
        IRepository<CommentModel> CommentModels { get; }
        IRepository<TeamModel> TeamModels { get; }

        void Commit();

        Task CommitAsync();
    }
}
