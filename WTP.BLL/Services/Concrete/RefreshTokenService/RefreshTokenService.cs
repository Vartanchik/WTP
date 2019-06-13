using System;
using System.Text;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.DTOs.TokensDTOs;
using WTP.DAL.Entities.AppUserEntities;
using WTP.DAL.UnitOfWork;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace WTP.BLL.Services.Concrete.RefreshTokenService
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IUnitOfWork _uow;
        private readonly TokenSettings _tokenSettings;

        public RefreshTokenService(IUnitOfWork unitOfWork, TokenSettings tokenSettings)
        {
            _uow = unitOfWork;
            _tokenSettings = tokenSettings;
        }

        public async Task<ServiceResult> UserVerifyAsync(LoginDto dto)
        {
            var user = await _uow.AppUsers.GetByEmailAsync(dto.Email);

            if (user != null && await _uow.AppUsers.CheckPasswordAsync(user.Id, dto.Password))
            {
                if (!await _uow.AppUsers.IsEmailConfirmedAsync(user.Id))
                {
                    return new ServiceResult("We sent you an confirmation email. Please confirm your registration.");
                }

                if (user.IsDeleted) return new ServiceResult("User is deleted.");

                return new ServiceResult();
            }

            return new ServiceResult("Incorrect email or password.");
        }

        public async Task<string> GenerateRefreshTokenAsync(int userId)
        {
            // delete curren user tokens
            await _uow.RefreshTokens.DeleteUserTokensAsync(userId);

            // create new token
            var refreshToken = new RefreshToken
            {
                Value = Guid.NewGuid().ToString("N"),
                CreatedDate = DateTime.UtcNow,
                UserId = userId,
                ExpiryTime = DateTime.UtcNow.Add(_tokenSettings.ExpireTime),
            };

            await _uow.RefreshTokens.CreateOrUpdate(refreshToken);
            await _uow.CommitAsync();

            return refreshToken.Value;
        }

        public async Task<string> GenerateAccessTokenAsync(int userId)
        {
            var secret = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenSettings.Secret));
            var roles = await _uow.AppUsers.GetRolesAsync(userId);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                        new Claim("UserID", userId.ToString())
                    }),

                SigningCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.UtcNow.Add(_tokenSettings.ExpireTime)
            };

            var newtoken = tokenHandler.CreateToken(tokenDescriptor);
            var encodedToken = tokenHandler.WriteToken(newtoken);

            return encodedToken;
        }

        public async Task<AccessDto> GetAccessAsync(string email)
        {
            var access = new AccessDto();

            var userId = await _uow.AppUsers.AsQueryable()
                                            .Where(u => u.Email == email)
                                            .Select(u => u.Id)
                                            .FirstOrDefaultAsync();

            if (userId == 0) return access;

            access.RefreshToken = await GenerateRefreshTokenAsync(userId);
            access.Token = await GenerateAccessTokenAsync(userId);

            return access;
        }

        public async Task<AccessDto> UpdateAccessAsync(AccessOperationDto dto)
        {
            var access = new AccessDto();

            var exist = await _uow.RefreshTokens.AsQueryable()
                                                .AnyAsync(t => t.UserId == dto.UserId &&
                                                               t.Value == dto.RefreshToken);

            if (exist)
            {
                access.RefreshToken = await GenerateRefreshTokenAsync(dto.UserId);
                access.Token = await GenerateAccessTokenAsync(dto.UserId);
            }

            return access;
        }
    }
}
