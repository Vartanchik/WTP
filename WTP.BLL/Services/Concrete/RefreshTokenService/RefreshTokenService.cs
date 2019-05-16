﻿using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.Models.RefreshToken;
using WTP.DAL.Entities;
using WTP.DAL.Repositories.ConcreteRepositories.RefreshTokenExtended;

namespace WTP.BLL.Services.Concrete.RefreshTokenService
{
    public class RefreshTokenService : IRefreshTokenService
    {
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
            var token = await _refreshToken.GetAsync(id);

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
    }
}
