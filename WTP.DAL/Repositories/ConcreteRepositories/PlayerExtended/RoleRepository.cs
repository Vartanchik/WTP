using WTP.DAL.Entities.PlayerEntities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.PlayerExtended
{
    internal class RoleRepository : RepositoryBase<Role>, IRepository<Role>
    {
        public RoleRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
