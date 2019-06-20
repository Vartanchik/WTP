using System;
using System.Threading.Tasks;
using WTP.DAL.Entities;
using WTP.DAL.Entities.AppUserEntities;
using WTP.DAL.Repositories.ConcreteRepositories.AppUserRepository;
using WTP.DAL.Repositories.ConcreteRepositories.RefreshTokenRepository;
using WTP.DAL.Repositories.ConcreteRepositories.RestoreTokenRepository;
using WTP.DAL.Repositories.GenericRepository;
using WTP.DAL.Entities.TeamEntities;

namespace WTP.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository<AppUser> AppUsers { get; }
        IRepository<Country> Countries { get; }
        IRepository<Gender> Genders { get; }
        IRepository<Language> Languages { get; }
        IRepository<Player> Players { get; }
        IRepository<Team> Teams { get; }
        IRefreshTokenRepository<RefreshToken> RefreshTokens { get; }
        IRepository<Game> Games { get; }
        IRepository<Server> Servers { get; }
        IRepository<Goal> Goals { get; }
        IRepository<Rank> Ranks { get; }
        IRestoreTokenRepository<RestoreToken> RestoreTokens { get; }
        IRepository<Invitation> Invitations { get; }

        //For Admin
        IRepository<History> Histories { get; }
        IRepository<Operation> Operations { get; }

        void Commit();

        Task CommitAsync();
    }
}
