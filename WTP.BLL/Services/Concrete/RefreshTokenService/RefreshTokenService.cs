using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.DAL.Entities.AppUserEntities;
using WTP.DAL.UnitOfWork;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace WTP.BLL.Services.Concrete.RefreshTokenService
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly TokenSettings _settings;

        public RefreshTokenService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _uow = unitOfWork;
            _mapper = mapper;
            _settings = new TokenSettings
            {
                Audience = configuration["AppSettings:Audience"],
                ExpireTime = TimeSpan.FromMinutes(Convert.ToDouble(60)),
                Secret = configuration["AppSettings:Secret"],
                Site = configuration["AppSettings:Site"]
            };
        }

        public async Task CreateAsync(RefreshTokenDto tokenDto)
        {
            var token = _mapper.Map<RefreshToken>(tokenDto);

            await _uow.RefreshTokens.CreateOrUpdate(token);
            await _uow.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _uow.RefreshTokens.DeleteAsync(id);
            await _uow.CommitAsync();
        }

        public async Task DeleteRangeAsync(int userId)
        {
            await _uow.RefreshTokens.DeleteUserTokensAsync(userId);
            await _uow.CommitAsync();
        }

        public async Task<RefreshTokenDto> GetAsync(int id)
        {
            var token = await _uow.RefreshTokens.GetByIdAsync(id);

            return _mapper.Map<RefreshTokenDto>(token);
        }

        public IQueryable<RefreshTokenDto> GetRangeAsync(int id)
        {
            var tokens = _uow.RefreshTokens.GetUserTokensAsync(id);
            var dto = new List<RefreshTokenDto>();

            foreach (var item in tokens)
            {
                dto.Add(_mapper.Map<RefreshTokenDto>(item));
            }

            return dto.AsQueryable();
        }

        public async Task<RefreshTokenDto> GetByUserIdAsync(int userId, string refreshToken)
        {
            var token = await _uow.RefreshTokens.GetByUserIdAsync(userId, refreshToken);

            return _mapper.Map<RefreshTokenDto>(token);
        }

        public async Task<ServiceResult> VerifyUser(LoginDto dto)
        {
            var user = await _uow.AppUsers.GetByEmailAsync(dto.Email);

            if (user != null && await _uow.AppUsers.CheckPasswordAsync(user.Id, dto.Password))
            {
                if (!await _uow.AppUsers.IsEmailConfirmedAsync(user.Id))
                {
                    return new ServiceResult("We sent you an confirmation email. Please confirm your registration.");
                }

                if (user.IsDeleted)
                {
                    return new ServiceResult(user.Email);
                }

                return new ServiceResult();
            }

            return new ServiceResult("Incorrect email or password.");
        }

        public async Task<AccessResponseDto> GetAccess(string email)
        {
            var userId = _uow.AppUsers.GetIdByCondition(u => u.Email == email);
            if (userId == 0) return null;

            var newRefreshToken = new RefreshTokenDto()
            {
                Value = Guid.NewGuid().ToString("N"),
                CreatedDate = DateTime.UtcNow,
                UserId = userId,
                ExpiryTime = DateTime.UtcNow.AddMinutes(60)
            };

            var oldRefreshTokens = GetRangeAsync(userId);

            if (oldRefreshTokens != null)
            {
                await DeleteRangeAsync(userId);
            }

            await CreateAsync(newRefreshToken);

            var accessToken = await CreateAccessToken(userId, newRefreshToken.Value);

            return accessToken;
        }

        //

        // warning. Indian programmers!

        //

        private async Task<AccessResponseDto> CreateAccessToken(int userId, string refreshToken)
        {
            var user = await _uow.AppUsers.GetByIdAsync(userId);
            if (user == null) return null;

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.Secret));

            var roles = await _uow.AppUsers.GetRolesAsync(userId);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                        new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                        new Claim("LoggedOn", DateTime.Now.ToString()),
                        new Claim("UserID", userId.ToString())
                     }),

                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Issuer = _settings.Site,
                Audience = _settings.Audience,
                Expires = DateTime.UtcNow.AddMinutes(60)
            };

            var newtoken = tokenHandler.CreateToken(tokenDescriptor);
            var encodedToken = tokenHandler.WriteToken(newtoken);

            return new AccessResponseDto()
            {
                Token = encodedToken,
                Expiration = newtoken.ValidTo,
                Refresh_token = refreshToken,
                Role = roles.FirstOrDefault(),
                UserName = user.UserName,
                Photo = user.Photo
            };
        }

        public async Task<ServiceResult> UpdateAccessToken(UpdateRefreshTokenDto dto)
        {
            var refreshToken = await GetByUserIdAsync(dto.UserId, dto.RefreshToken);

            if (refreshToken == null)
                return new ServiceResult("Refresh token not found.");
            if (refreshToken.ExpiryTime < DateTime.UtcNow)
                return new ServiceResult("Token expiration date.");

            var refreshTokenNew = new RefreshTokenDto()
            {
                Value = Guid.NewGuid().ToString("N"),
                CreatedDate = DateTime.UtcNow,
                UserId = dto.UserId,
                ExpiryTime = DateTime.UtcNow.AddMinutes(60)
            };

            await DeleteAsync(refreshToken.Id);

            await CreateAsync(refreshTokenNew);

            var accessToken = await CreateAccessToken(dto.UserId, refreshTokenNew.Value);

            return new ServiceResult();
        }

        public string GetCurrentTokenByUserId(int userId)
        {
            return _uow.RefreshTokens.AsQueryable()
                                     .Where(t => t.UserId == userId)
                                     .Select(x => x.Value)
                                     .FirstOrDefault();
        }
    }
}
