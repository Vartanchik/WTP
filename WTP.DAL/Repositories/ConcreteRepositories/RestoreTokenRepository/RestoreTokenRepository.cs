using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WTP.DAL.Entities.AppUserEntities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories.RestoreTokenRepository
{
    public class RestoreTokenRepository<IEntity> : RepositoryBase<RestoreToken>, IRestoreTokenRepository<RestoreToken>
    {
        private readonly ApplicationDbContext _context;

        public RestoreTokenRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<RestoreToken> GetByUserId(int userId)
        {
            return await _context.RestoreTokens.FirstOrDefaultAsync(x => x.AppUserId == userId);
        }
    }
}
