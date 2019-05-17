using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.Models.AppUserModels;
using WTP.BLL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.RefreshTokenService
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IUnitOfWork _uow;

        public RefreshTokenService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public Task CreateAsync(RefreshTokenModel tokenDto)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteRangeAsync(int userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<RefreshTokenModel> GetAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<RefreshTokenModel> GetByUserIdAsync(int userId, string refreshToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<RefreshTokenModel>> GetRangeAsync(int id)
        {
            throw new System.NotImplementedException();
        }
        /*
        private readonly IRefreshTokenRepository _refreshToken;
        private readonly IMapper _mapper;

        public RefreshTokenService(IRefreshTokenRepository refreshToken, IMapper mapper)
        {
            _refreshToken = refreshToken;
            _mapper = mapper;
        }

        public async Task CreateAsync(RefreshTokenModel tokenDto)
        {
            var token = _mapper.Map<RefreshToken>(tokenDto);

            await _refreshToken.CreateAsync(token);
        }

        public async Task DeleteAsync(int id)
        {
            await _refreshToken.DeleteAsync(id);
        }

        public async Task DeleteRangeAsync(int userId)
        {
            await _refreshToken.DeleteRangeAsync(userId);
        }

        public async Task<RefreshTokenModel> GetAsync(int id)
        {
            var token = await _refreshToken.GetByIdAsync(id);

            return _mapper.Map<RefreshTokenModel>(token);
        }

        public async Task<IEnumerable<RefreshTokenModel>> GetRangeAsync(int id)
        {
            var tokens = await _refreshToken.GetRangeAsync(id);

            return _mapper.Map<IEnumerable<RefreshTokenModel>>(tokens);
        }

        public async Task<RefreshTokenModel> GetByUserIdAsync(int userId, string refreshToken)
        {
            var token = await _refreshToken.GetByUserIdAsync(userId, refreshToken);

            return _mapper.Map<RefreshTokenModel>(token);
        }
        */
    }
}
