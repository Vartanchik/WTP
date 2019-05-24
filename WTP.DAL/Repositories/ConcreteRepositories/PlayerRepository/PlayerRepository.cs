using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.DAL.Entities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories.PlayerRepository
{
    public class PlayerRepository<IEntity> : RepositoryBase<Player>, IPlayerRepository<Player>
    {
        private readonly ApplicationDbContext _context;

        public PlayerRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<IList<Player>> GetListByUserIdAsync(int userId)
        {
            return await _context.Players
                .Include(p => p.Game)
                .Include(p => p.Server)
                .Include(p => p.Goal)
                .Include(p => p.Rank)
                .AsNoTracking()
                .Where(p => p.AppUserId == userId)
                .ToListAsync();
        }
    }
}
