using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.History;
using WTP.DAL.DomainModels;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.HistoryService
{
    public class HistoryService: IHistoryService
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
            var history = _mapper.Map<History>(historyDto);

            await _uow.Histories.CreateAsync(history);
        }
        public async Task UpdateAsync(HistoryDto historyDto)
        {
            var history = _mapper.Map<History>(historyDto);

            await _uow.Histories.UpdateAsync(history);
        }
        public async Task DeleteAsync(int id)
        {
            await _uow.Histories.DeleteAsync(id);
        }
        public async Task<HistoryDto> GetAsync(int id)
        {
            var history = await _uow.Histories.GetAsync(id);

            return _mapper.Map<HistoryDto>(history);
        }
        public async Task<IEnumerable<HistoryDto>> GetAllAsync()
        {
            var history = await _uow.Histories.GetAllAsync();

            return _mapper.Map<IEnumerable<HistoryDto>>(history);
        }
    }
}
