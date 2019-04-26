using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.Gender;
using WTP.DAL.DomainModels;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.GenderService
{
    public class GenderService : IGenderService
    {
        private IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GenderService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task CreateAsync(GenderDto genderDto)
        {
            var gender = _mapper.Map<Gender>(genderDto);

            await _uow.Genders.CreateAsync(gender);
        }
        public async Task UpdateAsync(GenderDto genderDto)
        {
            var gender = _mapper.Map<Gender>(genderDto);

            await _uow.Genders.UpdateAsync(gender);
        }
        public async Task DeleteAsync(int id)
        {
            await _uow.Genders.DeleteAsync(id);
        }
        public async Task<GenderDto> GetAsync(int id)
        {
            var gender = await _uow.Genders.GetAsync(id);

            return _mapper.Map<GenderDto>(gender);
        }
        public async Task<IEnumerable<GenderDto>> GetAllAsync()
        {
            var genders = await _uow.Genders.GetAllAsync();

            return _mapper.Map<IEnumerable<GenderDto>>(genders);
        }
    }
}
