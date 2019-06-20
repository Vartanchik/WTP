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
                                               .Where(t => t.AppUserId == userId);

            if (tokens != null)
            {
                _context.RefreshTokens.RemoveRange(tokens);

                await _context.SaveChangesAsync();
            }
        }
    }
}
