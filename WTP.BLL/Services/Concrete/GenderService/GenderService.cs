using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.Models.Gender;
using WTP.DAL.Entities;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.GenderService
{
    public class GenderService : IGenderService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GenderService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task CreateAsync(GenderModel genderDto)
        {
            var gender = _mapper.Map<Gender>(genderDto);

            await _uow.Genders.CreateAsync(gender);
        }
        public async Task UpdateAsync(GenderModel genderDto)
        {
            var gender = _mapper.Map<Gender>(genderDto);

            await _uow.Genders.UpdateAsync(gender);
        }
        public async Task DeleteAsync(int id)
        {
            await _uow.Genders.DeleteAsync(id);
        }
        public async Task<GenderModel> GetAsync(int id)
        {
            var gender = await _uow.Genders.GetAsync(id);

            return _mapper.Map<GenderModel>(gender);
        }
        public async Task<IEnumerable<GenderModel>> GetAllAsync()
        {
            var genders = await _uow.Genders.AsQueryable().ToListAsync();

            return _mapper.Map<IEnumerable<GenderModel>>(genders);
        }
    }
}
