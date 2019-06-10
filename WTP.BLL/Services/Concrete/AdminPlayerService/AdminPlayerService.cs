using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Shared;
using WTP.DAL.Entities;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.AdminPlayerService
{
    public class AdminPlayerService:IAdminPlayerService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public AdminPlayerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uow = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IList<PlayerShortDto>> GetJoinedPlayersListAsync()
        {
            var allPlayers = await _uow.Players.AsQueryable().ToListAsync();
            return _mapper.Map<IList<PlayerShortDto>>(allPlayers);
        }

        public IQueryable<Player> FilterByParam(Func<Player, bool> f, IQueryable<Player> baseQuery)
        {
            //if (!String.IsNullOrEmpty(name))
                return baseQuery.Where(f).AsQueryable();
            //return null;
        }

        public IQueryable<Player> SortByParam(PlayerSortState sortOrder, IQueryable<Player> baseQuery)
        {
            IQueryable<Player> query = Enumerable.Empty<Player>().AsQueryable();
            switch (sortOrder)
            {
                case PlayerSortState.NameDesc:
                    query = baseQuery.OrderByDescending(s => s.Name);
                    break;
                case PlayerSortState.NameAsc:
                    query = baseQuery.OrderBy(s => s.Name);
                    break;
                case PlayerSortState.EmailAsc:
                    query = baseQuery.OrderBy(s => s.AppUser.Email);
                    break;
                case PlayerSortState.EmailDesc:
                    query = baseQuery.OrderByDescending(s => s.AppUser.Email);
                    break;
                case PlayerSortState.IdAsc:
                    query = baseQuery.OrderBy(s => s.Id);
                    break;
                case PlayerSortState.IdDesc:
                    query = baseQuery.OrderByDescending(s => s.Id);
                    break;
                case PlayerSortState.UserIdAsc:
                    query = baseQuery.OrderBy(s => s.AppUserId);
                    break;
                case PlayerSortState.UserIdDesc:
                    query = baseQuery.OrderByDescending(s => s.AppUserId);
                    break;
                case PlayerSortState.UserNameAsc:
                    query = baseQuery.OrderBy(s => s.AppUser.UserName);
                    break;
                case PlayerSortState.UserNameDesc:
                    query = baseQuery.OrderByDescending(s => s.AppUser.UserName);
                    break;
                case PlayerSortState.TeamNameAsc:
                    query = baseQuery.OrderBy(s => s.Team.Name);
                    break;
                case PlayerSortState.TeamNameDesc:
                    query = baseQuery.OrderByDescending(s => s.Team.Name);
                    break;
                case PlayerSortState.GameNameAsc:
                    query = baseQuery.OrderBy(s => s.Game.Name);
                    break;
                case PlayerSortState.GameNameDesc:
                    query = baseQuery.OrderByDescending(s => s.Game.Name);
                    break;
                default:
                    query = baseQuery.OrderBy(s => s.Id);
                    break;
            }

            return query;
        }

        public IQueryable<Player> GetItemsOnPage(int page, int pageSize, IQueryable<Player> baseQuery)
        {
            IQueryable<Player> query = Enumerable.Empty<Player>().AsQueryable();
            query = baseQuery.Skip((page - 1) * pageSize).Take(pageSize);
            return query;
        }

        public async Task<int> GetCountOfPlayers()
        {
            return await _uow.Players.AsQueryable().CountAsync();
        }

        public async Task<PlayerManageDto> GetPageInfo(string name, int page, int pageSize,
            PlayerSortState sortOrder)
        {
            IQueryable<Player> query = _uow.Players.AsQueryable();
            IEnumerable<PlayerShortDto> items = Enumerable.Empty<PlayerShortDto>();

            try
            {
                var newQuery = FilterByParam(p => p.Name.Contains(name), query);
                //newQuery = FilterByParam(p => p.AppUser.UserName.Contains(name), query);
                //newQuery = FilterByParam(p => p.Id.Equals(Int32.Parse(name)), query);
                newQuery = SortByParam(sortOrder, newQuery);
                newQuery = GetItemsOnPage(page, pageSize, newQuery);

                items = _mapper.Map<List<PlayerShortDto>>(newQuery.ToList());
            }
            catch (ArgumentNullException ex)
            {
                //TODO
                //_log.Error(ex.Message);
            }

            var count = await this.GetCountOfPlayers();

            PlayerManageDto viewModel = new PlayerManageDto
            {
                PageViewModel = new PageDto(count, page, pageSize),
                SortViewModel = new PlayerManageSortDto(sortOrder),
                Players = items
            };
            return viewModel;
        }
    }
}
