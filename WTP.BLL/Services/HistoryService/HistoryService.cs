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
using WTP.Logging;

namespace WTP.BLL.Services.HistoryService
{
    public class HistoryService : IHistoryService
    {
        private IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILog _log;

        public HistoryService(IUnitOfWork uow, IMapper mapper, ILog log)
        {
            _uow = uow;
            _mapper = mapper;
            _log =log;
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

        public async Task<IQueryable<History>> FilterByUserName(string name, IQueryable<History> baseQuery)
        {            
            if (!String.IsNullOrEmpty(name))
                return baseQuery.Where(p => p.NewUserName.Contains(name));

            return null;
        }

        public async Task<IQueryable<History>> SortByParam(HistorySortState sortOrder, IQueryable<History> baseQuery)
        {
            IQueryable<History> query = Enumerable.Empty<History>().AsQueryable();
            switch (sortOrder)
            {
                case HistorySortState.NameDesc:
                    query = baseQuery.OrderByDescending(s => s.NewUserName);
                    break;
                case HistorySortState.EmailAsc:
                    query = baseQuery.OrderBy(s => s.NewUserEmail);
                    break;
                case HistorySortState.EmailDesc:
                    query = baseQuery.OrderByDescending(s => s.NewUserEmail);
                    break;
                case HistorySortState.IdAsc:
                    query = baseQuery.OrderBy(s => s.Id);
                    break;
                case HistorySortState.IdDesc:
                    query = baseQuery.OrderByDescending(s => s.Id);
                    break;
                case HistorySortState.UserIdAsc:
                    query = baseQuery.OrderBy(s => s.AppUserId);
                    break;
                case HistorySortState.UserIdDesc:
                    query = baseQuery.OrderByDescending(s => s.AppUserId);
                    break;
                case HistorySortState.AdminIdAsc:
                    query = baseQuery.OrderBy(s => s.AdminId);
                    break;
                case HistorySortState.AdminIdDesc:
                    query = baseQuery.OrderByDescending(s => s.AdminId);
                    break;
                case HistorySortState.DateAsc:
                    query = baseQuery.OrderBy(s => s.DateOfOperation);
                    break;
                case HistorySortState.NameAsc:
                    query = baseQuery.OrderBy(s => s.NewUserName);
                    break;
                default:
                    query = baseQuery.OrderByDescending(s => s.DateOfOperation);
                    break;
            }

            return query;
        }

        public async Task<IQueryable<History>> GetItemsOnPage(int page, int pageSize, IQueryable<History> baseQuery)
        {
            IQueryable<History> query = Enumerable.Empty<History>().AsQueryable();
            query = baseQuery.Skip((page - 1) * pageSize).Take(pageSize);
            return query;
        }

        public async Task<int> GetCountOfRecords()
        {
            return await _uow.Histories.AsQueryable().CountAsync();
        }

        public async Task<HistoryIndexDto> GetPageInfo(string name, int page, int pageSize,
            HistorySortState sortOrder)
        {
            IQueryable<History> query = _uow.Histories.AsQueryable();//Enumerable.Empty<History>().AsQueryable();
            IEnumerable<HistoryDto> items = Enumerable.Empty<HistoryDto>();
            int count = 0;
            try
            {
                var newQuery = await FilterByUserName(name, query);
                count = await newQuery.CountAsync();
                newQuery = await SortByParam(sortOrder, newQuery);
                newQuery = await GetItemsOnPage(page, pageSize, newQuery);

                items = _mapper.Map<List<HistoryDto>>(newQuery.ToList());
            }
            catch(ArgumentNullException ex)
            {
                //TODO
                //_log.Error(ex.Message);
            }
            
            //count = await this.GetCountOfRecords();

            HistoryIndexDto viewModel = new HistoryIndexDto
            {
                PageViewModel = new PageDto(count, page, pageSize),
                SortViewModel = new HistorySortDto(sortOrder),
                Histories = items
            };
            return viewModel;
        }
    }
}
