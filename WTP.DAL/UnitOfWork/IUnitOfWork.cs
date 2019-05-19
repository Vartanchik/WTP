using System;
using System.Threading.Tasks;
using WTP.DAL.Entities;
using WTP.DAL.Repositories.ConcreteRepositories;
using WTP.DAL.Repositories.GenericRepository;

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
        ITokenRepository<RefreshToken> Tokens { get; }

        void Commit();

        Task CommitAsync();
    }
}
