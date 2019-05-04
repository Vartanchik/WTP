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
            var record = _mapper.Map<History>(historyDto);

            await _uow.Histories.CreateAsync(record);
        }
        public async Task UpdateAsync(HistoryDto genderDto)
        {
            var record = _mapper.Map<History>(genderDto);

            await _uow.Histories.UpdateAsync(record);
        }
        public async Task DeleteAsync(int id)
        {
            await _uow.Histories.DeleteAsync(id);
        }
        public async Task<HistoryDto> GetAsync(int id)
        {
            var record = await _uow.Histories.GetAsync(id);

            return _mapper.Map<HistoryDto>(record);
        }
        public async Task<IEnumerable<HistoryDto>> GetAllAsync()
        {
            var records = await _uow.Histories.GetAllAsync();

            return _mapper.Map<IEnumerable<HistoryDto>>(records);
        }
    }
}
