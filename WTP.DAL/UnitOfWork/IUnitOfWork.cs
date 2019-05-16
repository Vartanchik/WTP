using System;
using System.Threading.Tasks;
using WTP.DAL.Entities;
using WTP.DAL.Entities.AppUserEntities;
using WTP.DAL.Entities.PlayerEntities;
using WTP.DAL.Entities.TeamEntities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<AppUser> AppUsers { get; }
        IRepository<DeletedUser> DeletedUsers { get; }
        IRepository<RefreshToken> Tokens { get; }
        IRepository<Country> Countries { get; }
        IRepository<Gender> Genders { get; }
        IRepository<Language> Languages { get; }
        IRepository<Player> Players { get; }
        IRepository<Server> Servesr { get; }
        IRepository<Goal> Goals { get; }
        IRepository<Role> Roles { get; }
        IRepository<Rank> Ranks { get; }
        IRepository<Comment> Comments { get; }
        IRepository<Team> Teams { get; }

        void Commit();

        Task CommitAsync();
    }
}
