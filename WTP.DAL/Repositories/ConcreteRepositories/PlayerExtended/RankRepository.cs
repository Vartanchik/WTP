using WTP.DAL.Entities.PlayerEntities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.PlayerExtended
{
    internal class RankRepository : RepositoryBase<Rank>, IRepository<Rank>
    {
        public RankRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
