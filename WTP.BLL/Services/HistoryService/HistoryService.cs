using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Shared;
using WTP.DAL.Entities.AppUserEntities;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.HistoryService
{
    public class HistoryService : IHistoryService
    {
        private IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public HistoryService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task CreateAsync(HistoryDto historyDto)
        {
            var record = _mapper.Map<History>(historyDto);

            await _uow.Histories.CreateOrUpdate(record);
        }

        public async Task UpdateAsync(HistoryDto genderDto)
        {
            var record = _mapper.Map<History>(genderDto);

            await _uow.Histories.CreateOrUpdate(record);
        }

        public async Task DeleteAsync(int id)
        {
            await _uow.Histories.DeleteAsync(id);
        }

        public async Task<HistoryDto> GetAsync(int id)
        {
            var record = await _uow.Histories.GetByIdAsync(id); 

            return _mapper.Map<HistoryDto>(record);
        }

        public async Task<IList<HistoryDto>> GetHistoryList()
        {
            var records = await _uow.Histories.AsQueryable().ToListAsync();

            return _mapper.Map<IList<HistoryDto>>(records);
        }

        public IList<HistoryDto> FilterByUserName(List<HistoryDto> histories, string name)
        {
            if (histories == null)
                return null;

            if (!String.IsNullOrEmpty(name))
            {
                histories = histories.Where(p => p.NewUserName.Contains(name)).ToList();
            }

            return histories;
        }

        public IList<HistoryDto> SortByParam(List<HistoryDto> histories, HistorySortState sortOrder)
        {
            if (histories == null)
                return null;

            switch (sortOrder)
            {
                case HistorySortState.NameDesc:
                    histories = histories.OrderByDescending(s => s.NewUserName).ToList();
                    break;
                case HistorySortState.EmailAsc:
                    histories = histories.OrderBy(s => s.NewUserEmail).ToList();
                    break;
                case HistorySortState.EmailDesc:
                    histories = histories.OrderByDescending(s => s.NewUserEmail).ToList();
                    break;
                case HistorySortState.IdAsc:
                    histories = histories.OrderBy(s => s.Id).ToList();
                    break;
                case HistorySortState.IdDesc:
                    histories = histories.OrderByDescending(s => s.Id).ToList();
                    break;
                case HistorySortState.UserIdAsc:
                    histories = histories.OrderBy(s => s.AppUserId).ToList();
                    break;
                case HistorySortState.UserIdDesc:
                    histories = histories.OrderByDescending(s => s.AppUserId).ToList();
                    break;
                case HistorySortState.AdminIdAsc:
                    histories = histories.OrderBy(s => s.AdminId).ToList();
                    break;
                case HistorySortState.AdminIdDesc:
                    histories = histories.OrderByDescending(s => s.AdminId).ToList();
                    break;
                case HistorySortState.DateAsc:
                    histories = histories.OrderBy(s => s.DateOfOperation).ToList();
                    break;
                case HistorySortState.NameAsc:
                    histories = histories.OrderBy(s => s.NewUserName).ToList();
                    break;
                default:
                    histories = histories.OrderByDescending(s => s.DateOfOperation).ToList();
                    break;
            }

            return histories;
        }

        public async Task<List<HistoryDto>> GetItemsOnPage(int page, int pageSize)
        {
            var items = await _uow.Histories.AsQueryable().Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return _mapper.Map<List<HistoryDto>>(items);
        }

        public async Task<int> GetCountOfRecords()
        {
            return await _uow.Histories.AsQueryable().CountAsync();
        }

        public async Task<HistoryIndexDto> GetPageInfo(string name, int page, int pageSize,
            HistorySortState sortOrder)
        {
            //int pageSize = 3;
            var items = await this.GetItemsOnPage(page, pageSize);
            var count = await this.GetCountOfRecords();
            items = this.FilterByUserName(items, name).ToList();
            items = this.SortByParam(items, sortOrder).ToList();

            HistoryIndexDto viewModel = new HistoryIndexDto
            {
                PageViewModel = new PageDto(count, page, pageSize),
                SortViewModel = new HistorySortDto(sortOrder),
                //FilterViewModel = new UserFilterViewModel(users/*(List<AppUserDto>)await _appUserService.GetAllUsersAsync()*/, name),
                Histories = items
            };
            return viewModel;
        }
    }
}
