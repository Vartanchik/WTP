using WTP.DAL.Entities.AppUserEntities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories.AppUserExtended
{
    internal class RefreshTokenRepository : RepositoryBase<RefreshToken>, IRepository<RefreshToken>
    {
        public RefreshTokenRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
