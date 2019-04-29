using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.Admin;
using WTP.DAL.DomainModels;
using WTP.DAL.Repositories.ConcreteRepositories.AdminExtended;
using WTP.DAL.Repositories.ConcreteRepositories.AppUserExtended;

namespace WTP.BLL.Services.Concrete.AdminService
{
    public class AdminService: IAdminService
    {
        private readonly IMapper _mapper;
        private readonly IAdminRepository _adminRepository;

        public AdminService(IMapper mapper, IAdminRepository appUserRepository)
        {
            _mapper = mapper;
            _adminRepository = appUserRepository;
        }

        public async Task<IdentityResult> CreateAsync(AdminDto adminDto, string password)
        {
            var admin = _mapper.Map<Admin>(adminDto);

            var result = await _adminRepository.CreateAsync(admin, password);

            return result;
        }

        public async Task<AdminDto> GetAsync(int id)
        {
            var admin = await _adminRepository.GetAsync(id);

            var adminDto = _mapper.Map<AdminDto>(admin);

            return adminDto;
        }

        public async Task<AdminDto> GetByEmailAsync(string email)
        {
            var admin = await _adminRepository.GetByEmailAsync(email);

            return _mapper.Map<AdminDto>(admin);
        }

        public async Task<IdentityResult> UpdateAsync(AdminDto adminDto)
        {
            var admin = _mapper.Map<Admin>(adminDto);

            var result = await _adminRepository.UpdateAsync(admin);

            return result;
        }

        public async Task<IList<string>> GetRolesAsync(AdminDto adminDto)
        {
            var admin = _mapper.Map<Admin>(adminDto);

            return await _adminRepository.GetRolesAsync(admin);
        }

        public async Task<bool> CheckPasswordAsync(int id, string password)
        {
            return await _adminRepository.CheckPasswordAsync(id, password);
        }
    }
}
