using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.DAL.Entities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories.RefreshTokenExtended
{
    public class RefreshTokenRepository : RepositoryBase<RefreshToken>, IRepository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task CreateAsync(RefreshToken item)
        {
            await _context.RefreshTokens.AddAsync(item);

            await _context.SaveChangesAsync();
        }

        public override async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Deleted;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteRangeAsync(int userId)
        {
            var tokens = await GetRangeAsync(userId);

            if (tokens != null)
            {
                _context.RefreshTokens.RemoveRange(tokens);

                await _context.SaveChangesAsync();
            }
        }

        public override async Task<RefreshToken> GetAsync(int id)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(_ => _.Id == id);
        }

        public async Task<IEnumerable<RefreshToken>> GetRangeAsync(int id)
        {
            return await _context.RefreshTokens.Where(_ => _.UserId == id).ToListAsync();
        }

        public async Task<RefreshToken> GetByUserIdAsync(int userId, string refreshToken)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(_ => _.UserId == userId && _.Value == refreshToken);
        }
    }
}
