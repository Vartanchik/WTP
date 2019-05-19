using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.DAL.Entities;
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

            await _uow.Tokens.CreateAsync(token);
        }

        public async Task DeleteAsync(int id)
        {
            await _uow.Tokens.DeleteAsync(id);
        }

        public async Task DeleteRangeAsync(int userId)
        {
            await _uow.Tokens.DeleteRangeAsync(userId);
        }

        public async Task<RefreshTokenDto> GetAsync(int id)
        {
            var token = await _uow.Tokens.GetAsync(id);

            return _mapper.Map<RefreshTokenDto>(token);
        }

        public async Task<IEnumerable<RefreshTokenDto>> GetRangeAsync(int id)
        {
            var tokens = await _uow.Tokens.GetRangeAsync(id);

            return _mapper.Map<IEnumerable<RefreshTokenDto>>(tokens);
        }

        public async Task<RefreshTokenDto> GetByUserIdAsync(int userId, string refreshToken)
        {
            var token = await _uow.Tokens.GetByUserIdAsync(userId, refreshToken);

            return _mapper.Map<RefreshTokenDto>(token);
        }
    }
}
