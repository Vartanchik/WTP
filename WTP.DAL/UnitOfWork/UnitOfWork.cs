using System;
using System.Threading.Tasks;
using WTP.DAL.Entities;
using WTP.DAL.Repositories.GenericRepository;
using Microsoft.AspNetCore.Identity;
using WTP.DAL.Repositories.ConcreteRepositories;
using WTP.DAL.Entities.AppUserEntities;

namespace WTP.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _identityService;
        private IUserRepository<AppUser> _appUsers;
        private IRepository<Country> _countries;
        private IRepository<Gender> _genders;
        private IRepository<Language> _languages;
        private IPlayerRepository<Player> _players;
        private IRepository<Team> _teams;
        private ITokenRepository<RefreshToken> _tokens;
        private IRepository<Comment> _comments;
        private IRepository<Match> _matches;
        private IRepository<Game> _games;
        private IRepository<History> _histories;
        private IRepository<Operation> _operations;
        private IRepository<Goal> _goals;
        private IRepository<Rank> _ranks;
        private IRepository<Server> _servers;
        private bool _disposed = false;

        public UnitOfWork(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _identityService = userManager;
        }

        public IUserRepository<AppUser> AppUsers => _appUsers ?? (_appUsers = new UserRepository<AppUser>(_context, _identityService));
        public IRepository<Country> Countries => _countries ?? (_countries = new RepositoryBase<Country>(_context));
        public IRepository<Gender> Genders => _genders ?? (_genders = new RepositoryBase<Gender>(_context));
        public IRepository<Language> Languages => _languages ?? (_languages = new RepositoryBase<Language>(_context));
        public IPlayerRepository<Player> Players => _players ?? (_players = new PlayerRepository<Player>(_context));
        public IRepository<Team> Teams => _teams ?? (_teams = new RepositoryBase<Team>(_context));
        public ITokenRepository<RefreshToken> Tokens => _tokens ?? (_tokens = new TokenRepository<RefreshToken>(_context));
        public IRepository<Comment> Comments => _comments ?? (_comments = new RepositoryBase<Comment>(_context));
        public IRepository<Match> Matches => _matches ?? (_matches = new RepositoryBase<Match>(_context));
        public IRepository<Game> Games => _games ?? (_games = new RepositoryBase<Game>(_context));
        public IRepository<History> Histories => _histories ?? (_histories = new RepositoryBase<History>(_context));
        public IRepository<Operation> Operations => _operations ?? (_operations = new RepositoryBase<Operation>(_context));
        public IRepository<Goal> Goals => _goals ?? (_goals = new RepositoryBase<Goal>(_context));
        public IRepository<Rank> Ranks => _ranks ?? (_ranks = new RepositoryBase<Rank>(_context));
        public IRepository<Server> Servers => _servers ?? (_servers = new RepositoryBase<Server>(_context));

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
