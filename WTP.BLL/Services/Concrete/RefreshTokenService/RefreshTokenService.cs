using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.DAL.Entities;
using WTP.DAL.Entities.AppUserEntities;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.RefreshTokenService
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public RefreshTokenService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uow = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateAsync(RefreshTokenDto tokenDto)
        {
            var token = _mapper.Map<RefreshToken>(tokenDto);

            await _uow.RefreshTokens.CreateOrUpdate(token);
        }

        public async Task DeleteAsync(int id)
        {
            await _uow.RefreshTokens.DeleteAsync(id);
        }

        public async Task DeleteRangeAsync(int userId)
        {
            await _uow.RefreshTokens.DeleteUserTokensAsync(userId);
        }

        public async Task<RefreshTokenDto> GetAsync(int id)
        {
            var token = await _uow.RefreshTokens.GetByIdAsync(id);

            return _mapper.Map<RefreshTokenDto>(token);
        }

        public IQueryable<RefreshTokenDto> GetRangeAsync(int id)
        {
            var tokens = _uow.RefreshTokens.GetUserTokensAsync(id);
            var dto = new List<RefreshTokenDto>();

            foreach (var item in tokens)
            {
                dto.Add(_mapper.Map<RefreshTokenDto>(item));
            }

            return dto.AsQueryable();
        }

        public async Task<RefreshTokenDto> GetByUserIdAsync(int userId, string refreshToken)
        {
            var token = await _uow.RefreshTokens.GetByUserIdAsync(userId, refreshToken);

            return _mapper.Map<RefreshTokenDto>(token);
        }
    }
}
