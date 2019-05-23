using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.DAL.Entities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    public class PlayerRepository<IEntity> : RepositoryBase<Player>, IPlayerRepository<Player>
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Player> _dbset;

        public PlayerRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
            _dbset = context.Set<Player>();
        }

        public override async Task<Player> GetAsync(int userId)
        {
            var player = await _context.Players.Include(x => x.AppUser).Include(x => x.Game)
                .Include(x => x.Goal).Include(x => x.Server).Include(x => x.Team)
                .FirstOrDefaultAsync(userInc => userInc.Id == userId);

            return player;
        }

        public async Task<IList<Player>> GetAllPlayersAsync()
        {
            var player = await _context.Players.Include(x => x.Game)
            .Include(x => x.Goal).Include(x => x.Server).Include(x => x.Team)
            .ToListAsync();
            return player;
        }
    }
}
