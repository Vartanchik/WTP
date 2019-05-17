using AutoMapper;
using System;
using System.Threading.Tasks;
using WTP.BLL.Models;
using WTP.BLL.Models.AppUserModels;
using WTP.BLL.Models.PlayerModels;
using WTP.BLL.Models.TeamModels;
using WTP.BLL.UnitOfWork;
using WTP.DAL.Repositories;

namespace WTP.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private bool _disposed = false;

        private IRepository<AppUserModel> _appUserModels;
        private IRepository<RefreshTokenModel> _tokenModels;
        private IRepository<CountryModel> _countrieModels;
        private IRepository<GenderModel> _genderModels;
        private IRepository<LanguageModel> _languageModels;
        private IRepository<PlayerModel> _playerModels;
        private IRepository<ServerModel> _servesr;
        private IRepository<GoalModel> _goalModels;
        private IRepository<AppUserRoleModel> _appUserRoleModels;
        private IRepository<PlayerRoleModel> _playerRoleModels;
        private IRepository<RankModel> _rankModels;
        private IRepository<CommentModel> _commentModels;
        private IRepository<TeamModel> _teamModels;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<AppUserModel> AppUserModels => _appUserModels ?? (_appUserModels = new RepositoryBase<AppUserModel>(_context, _mapper));
        public IRepository<RefreshTokenModel> TokenModels => _tokenModels ?? (_tokenModels = new RepositoryBase<RefreshTokenModel>(_context, _mapper));
        public IRepository<CountryModel> CountrieModels => _countrieModels ?? (_countrieModels = new RepositoryBase<CountryModel>(_context, _mapper));
        public IRepository<GenderModel> GenderModels => _genderModels ?? (_genderModels = new RepositoryBase<GenderModel>(_context, _mapper));
        public IRepository<LanguageModel> LanguageModels => _languageModels ?? (_languageModels = new RepositoryBase<LanguageModel>(_context, _mapper));
        public IRepository<PlayerModel> PlayerModels => _playerModels ?? (_playerModels = new RepositoryBase<PlayerModel>(_context, _mapper));
        public IRepository<ServerModel> Servesr => _servesr ?? (_servesr = new RepositoryBase<ServerModel>(_context, _mapper));
        public IRepository<GoalModel> GoalModels => _goalModels ?? (_goalModels = new RepositoryBase<GoalModel>(_context, _mapper));
        public IRepository<AppUserRoleModel> AppUserRoleModels => _appUserRoleModels ?? (_appUserRoleModels = new RepositoryBase<AppUserRoleModel>(_context, _mapper));
        public IRepository<PlayerRoleModel> PlayerRoleModels => _playerRoleModels ?? (_playerRoleModels = new RepositoryBase<PlayerRoleModel>(_context, _mapper));
        public IRepository<RankModel> RankModels => _rankModels ?? (_rankModels = new RepositoryBase<RankModel>(_context, _mapper));
        public IRepository<CommentModel> CommentModels => _commentModels ?? (_commentModels = new RepositoryBase<CommentModel>(_context, _mapper));
        public IRepository<TeamModel> TeamModels => _teamModels ?? (_teamModels = new RepositoryBase<TeamModel>(_context, _mapper));

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
