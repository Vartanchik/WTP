﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WTP.DAL.Entities.AppUserEntities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories.RefreshTokenRepository
{
    public class RefreshTokenRepository<IEntity> : RepositoryBase<RefreshToken>, IRefreshTokenRepository<RefreshToken>
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task DeleteUserTokensAsync(int userId)
        {
            var tokens = _context.RefreshTokens.AsQueryable()
                                               .Where(t => t.UserId == userId);

            if (tokens != null)
            {
                _context.RefreshTokens.RemoveRange(tokens);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<RefreshToken> UserTokenByValue(int userId, string refreshToken)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userId && 
                                                                         t.Value == refreshToken);
        }

        public IQueryable<RefreshToken> GetUserTokensAsync(int id)
        {
            return _context.RefreshTokens.AsQueryable()
                                         .Where(t => t.UserId == id);
        }

        public virtual int GetIdByCondition(Func<RefreshToken, bool> condition)
        {
            return base.AsQueryable()
                       .Where(condition)
                       .Select(x => x.Id)
                       .FirstOrDefault();
        }
    }
}
