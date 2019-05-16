using WTP.DAL.Entities.PlayerEntities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.PlayerExtended
{
    internal class CommentRepository : RepositoryBase<Comment>, IRepository<Comment>
    {
        public CommentRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
