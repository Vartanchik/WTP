using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WTP.DAL.Entities.TeamEntities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories.InvitationRepository
{
    public class InvitationRepository : RepositoryBase<Invitation>, IRepository<Invitation>
    {
        public InvitationRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public override async Task<Invitation> GetByIdAsync(int id)
        {
            return await base.AsQueryable()
                             .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
