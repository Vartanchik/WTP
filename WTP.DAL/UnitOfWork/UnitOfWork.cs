using System;
using System.Threading.Tasks;
using WTP.DAL.DomainModels;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IRepository<AppUser> _appUsers;
        private IRepository<Country> _countries;
        private IRepository<Gender> _genders;
        private IRepository<Language> _languages;
        private IRepository<Player> _players;
        private IRepository<Team> _teams;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<AppUser> AppUsers => _appUsers ?? (_appUsers = new RepositoryBase<AppUser>(_context));
        public IRepository<Country> Countries => _countries ?? (_countries = new RepositoryBase<Country>(_context));
        public IRepository<Gender> Genders => _genders ?? (_genders = new RepositoryBase<Gender>(_context));
        public IRepository<Language> Languages => _languages ?? (_languages = new RepositoryBase<Language>(_context));
        public IRepository<Player> Players => _players ?? (_players = new RepositoryBase<Player>(_context));
        public IRepository<Team> Teams => _teams ?? (_teams = new RepositoryBase<Team>(_context));

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
