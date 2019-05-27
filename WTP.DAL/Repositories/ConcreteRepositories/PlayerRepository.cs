using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WTP.DAL.Entities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    class PlayerRepository<IEntity> : RepositoryBase<Player>, IPlayerRepository<Player>
    {
        private readonly ApplicationDbContext _context;

        public PlayerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override IQueryable<Player> AsQueryable()
        {
            return _context.Players.Include(s => s.Server).Include(g => g.Game).Include(t => t.Team).Include(u => u.AppUser)
                .ThenInclude(p=>p.Players)
                .Include(r => r.Rank).AsQueryable();
        }
    }
}
