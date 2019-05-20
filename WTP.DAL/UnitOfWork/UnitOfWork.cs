using System;
using System.Threading.Tasks;
using WTP.DAL.Entities;
using WTP.DAL.Repositories.GenericRepository;
using Microsoft.AspNetCore.Identity;
using WTP.DAL.Repositories.ConcreteRepositories;

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
        private IRepository<Player> _players;
        private IRepository<Team> _teams;
        private ITokenRepository<RefreshToken> _tokens;
        private IRepository<Game> _games;
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
        public IRepository<Player> Players => _players ?? (_players = new RepositoryBase<Player>(_context));
        public IRepository<Team> Teams => _teams ?? (_teams = new RepositoryBase<Team>(_context));
        public ITokenRepository<RefreshToken> Tokens => _tokens ?? (_tokens = new TokenRepository<RefreshToken>(_context));
        public IRepository<Game> Games => _games ?? (_games = new RepositoryBase<Game>(_context));

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
