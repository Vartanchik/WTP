using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.Operation;
using WTP.DAL.DomainModels;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.OperationService
{
    public class OperationService:IOperationService
    {
        private IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public OperationService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task CreateAsync(OperationDto operationDto)
        {
            var operation = _mapper.Map<Operation>(operationDto);

            await _uow.Operations.CreateAsync(operation);
        }
        public async Task UpdateAsync(OperationDto operationDto)
        {
            var operation = _mapper.Map<Operation>(operationDto);

            await _uow.Operations.UpdateAsync(operation);
        }
        public async Task DeleteAsync(int id)
        {
            await _uow.Operations.DeleteAsync(id);
        }
        public async Task<OperationDto> GetAsync(int id)
        {
            var operation = await _uow.Operations.GetAsync(id);

            return _mapper.Map<OperationDto>(operation);
        }
        public async Task<IEnumerable<OperationDto>> GetAllAsync()
        {
            var operations = await _uow.Genders.GetAllAsync();

            return _mapper.Map<IEnumerable<OperationDto>>(operations);
        }
    }
}
