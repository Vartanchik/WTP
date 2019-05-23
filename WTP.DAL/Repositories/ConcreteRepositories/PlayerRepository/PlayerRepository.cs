using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return _context.Players
                .Include(player => player.AppUser)
                    .ThenInclude(appUser => appUser.Country)
                .Include(player => player.AppUser)
                    .ThenInclude(appUser => appUser.Gender)
                .Include(player => player.AppUser)
                    .ThenInclude(appUser => appUser.AppUserLanguages)
                        .ThenInclude(appUserLanguages => appUserLanguages.Language)
                .Include(player => player.Game)
                .Include(player => player.Server)
                .Include(player => player.Goal)
                .Include(player => player.Rank)
                .AsNoTracking()
                .Where(player => player.AppUserId == userId)
                .ToList();
        }
    }
}
