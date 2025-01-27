using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WealthFlow.Application.Caching.Interfaces;
using WealthFlow.Application.Security.Interfaces;
using WealthFlow.Application.Users.DTOs;
using WealthFlow.Infrastructure.Caching;
using static WealthFlow.Domain.Enums.Enum;
using WealthFlow.Shared.Helpers;
using System.Net;
using WealthFlow.Domain.Entities;

namespace WealthFlow.Application.Security.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ICacheService _cacheService;

        public TokenService(IConfiguration configuration, ICacheService cacheService)
        {
            _configuration = configuration;
            _cacheService = cacheService;
        }
        public async Task<Result> GenerateJwtToken(UserDTO user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            if(await StoreToken(jwtToken, user.Id))
                return Result.Failure("Couldn't to process.Please log in again", HttpStatusCode.InternalServerError);

            return Result.Success(jwtToken, HttpStatusCode.OK);
        }

        private async Task<bool> StoreToken(string token, Guid userId)
        {
            string jwtTokenKey = $"jwt-token:{userId}";
            TimeSpan expirationTime = ToTimeSpan.covertToTimeSpan(ExpirationType.JWT_TOKEN_VERIFICATION, TimeUnitConversion.DAYS);

            return await _cacheService.StoreAsync(jwtTokenKey, token, expirationTime);
        }

        public async Task<bool> IsValidatedJwtToken(string  jwtKey)
        {
            var jwtToken = await _cacheService.GetAsync(jwtKey);

            if (string.IsNullOrEmpty(jwtToken))
                return false;

            return true;
        }

        public async Task<Guid?> GetUserIdFromJwtTokenIfValidated(string jwtKey)
        {
            var token = await _cacheService.GetAsync(jwtKey);

            if (string.IsNullOrEmpty(token))
                return null;

            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                return null;

            return userId;
        }

        public async Task<bool> StorePasswordResetToken(Guid userId, string token)
        {
            TimeSpan expirationTime = ToTimeSpan.covertToTimeSpan(ExpirationType.PASSWORD_VERIFICATION, TimeUnitConversion.MINUTES);
            string verificationKey = $"password_reset_{token}";

            return await _cacheService.StoreAsync(verificationKey, userId.ToString(), expirationTime);
        }

        public async Task<string?> GetPasswordResetTokenIfAny(string key)
        {
            string storedKey = $"password_reset_{key}";
            string userId = await _cacheService.GetAsync(storedKey);

            await _cacheService.RemoveAsync(storedKey);

            return userId;
        }
        public string GetJwtToken()
        {
            throw new NotImplementedException();
        }
    }
}
