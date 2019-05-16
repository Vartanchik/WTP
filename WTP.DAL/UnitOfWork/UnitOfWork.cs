using System;
using System.Threading.Tasks;
using WTP.DAL.Entities;
using WTP.DAL.Entities.AppUserEntities;
using WTP.DAL.Entities.PlayerEntities;
using WTP.DAL.Entities.TeamEntities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext   _context;
        private IRepository<AppUser>            _appUsers;
        private IRepository<DeletedUser>        _deletedUsers;
        private IRepository<RefreshToken>       _tokens;
        private IRepository<Country>            _countries;
        private IRepository<Gender>             _genders;
        private IRepository<Language>           _languages;
        private IRepository<Player>             _players;
        private IRepository<Server>             _servesr;
        private IRepository<Goal>               _goals;
        private IRepository<Role>               _roles;
        private IRepository<Rank>               _ranks;
        private IRepository<Comment>            _comments;
        private IRepository<Team>               _teams;
        private bool                            _disposed = false;

        public IRepository<AppUser>         AppUsers => _appUsers ?? (_appUsers = new RepositoryBase<AppUser>(_context));
        public IRepository<DeletedUser>     DeletedUsers => _deletedUsers ?? (_deletedUsers = new RepositoryBase<DeletedUser>(_context));
        public IRepository<RefreshToken>    Tokens => _tokens ?? (_tokens = new RepositoryBase<RefreshToken>(_context));
        public IRepository<Country>         Countries => _countries ?? (_countries = new RepositoryBase<Country>(_context));
        public IRepository<Gender>          Genders => _genders ?? (_genders = new RepositoryBase<Gender>(_context));
        public IRepository<Language>        Languages => _languages ?? (_languages = new RepositoryBase<Language>(_context));
        public IRepository<Player>          Players => _players ?? (_players = new RepositoryBase<Player>(_context));
        public IRepository<Server>          Servesr => _servesr ?? (_servesr = new RepositoryBase<Server>(_context));
        public IRepository<Goal>            Goals => _goals ?? (_goals = new RepositoryBase<Goal>(_context));
        public IRepository<Role>            Roles => _roles ?? (_roles = new RepositoryBase<Role>(_context));
        public IRepository<Rank>            Ranks => _ranks ?? (_ranks = new RepositoryBase<Rank>(_context));
        public IRepository<Comment>         Comments => _comments ?? (_comments = new RepositoryBase<Comment>(_context));
        public IRepository<Team>            Teams => _teams ?? (_teams = new RepositoryBase<Team>(_context));

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

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
