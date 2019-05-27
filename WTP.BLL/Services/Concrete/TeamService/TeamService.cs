using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.DTOs.TeamDTOs;
using WTP.DAL.Entities;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.TeamService
{
    public class TeamService : ITeamService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public TeamService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uow = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IList<TeamDto>> GetTeamsListAsync()
        {
            var listOfTeams = await _uow.Teams.AsQueryable().ToListAsync();
            return _mapper.Map<IList<TeamDto>>(listOfTeams);
        }

        public async Task CreateOrUpdateAsync(TeamDto dto, int? adminId = null)
        {
            var team = _mapper.Map<Team>(dto);

            await _uow.Teams.CreateOrUpdate(team);
            await _uow.CommitAsync();
        }

        public async Task DeleteAsync(int teamId, int? adminId = null)
        {
            await _uow.Teams.DeleteAsync(teamId);
            await _uow.CommitAsync();
        }

        public async Task<TeamDto> FindAsync(int teamId)
        {
            return _mapper.Map<TeamDto>(await _uow.Teams.GetAsync(teamId));
        }
    }
}
