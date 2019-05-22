using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
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

        public IList<Player> GetPlayersByUserId(int userId)
        {
            _context.Players.Include(x => x.AppUser).Include(x => x.Game).Include(x => x.Server).Include(x => x.Goal).Include(x => x.Rank);
            return null;
        }
    }
}
