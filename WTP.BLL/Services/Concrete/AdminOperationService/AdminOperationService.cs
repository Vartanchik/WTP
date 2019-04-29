using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.AdminOperation;
using WTP.DAL.DomainModels;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.AdminOperationService
{
    public class AdminOperationService: IAdminOperationService
    {
        private IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public AdminOperationService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task CreateAsync(AdminOperationDto adminOperationDto)
        {
            var operation = _mapper.Map<AdminOperation>(adminOperationDto);

            await _uow.AdminOperations.CreateAsync(operation);
        }

        public async Task UpdateAsync(AdminOperationDto adminOperationDto)
        {
            var operation = _mapper.Map<AdminOperation>(adminOperationDto);

            await _uow.AdminOperations.UpdateAsync(operation);
        }

        public async Task DeleteAsync(int id)
        {
            await _uow.AdminOperations.DeleteAsync(id);
        }

        public async Task<AdminOperationDto> GetAsync(int id)
        {
            var operation = await _uow.AdminOperations.GetAsync(id);

            return _mapper.Map<AdminOperationDto>(operation);
        }

        public async Task<IEnumerable<AdminOperationDto>> GetAllAsync()
        {
            var operation = await _uow.AdminOperations.GetAllAsync();

            return _mapper.Map<IEnumerable<AdminOperationDto>>(operation);
        }
    }
}
