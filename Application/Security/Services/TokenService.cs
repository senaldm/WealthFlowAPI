using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WealthFlow.Application.Caching.Interfaces;
using WealthFlow.Application.Security.Interfaces;
using WealthFlow.Application.Users.DTOs;
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
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenService(IConfiguration configuration, ICacheService cacheService, IHttpContextAccessor contextAccessor)
        {
            _configuration = configuration;
            _cacheService = cacheService;
            _contextAccessor = contextAccessor;
        }
        public async Task<Result> GenerateJwtToken(UserDTO user)
        {
            var jwtToken = CreateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            string refreshTokenKey = $"refresh-token:{user.Id}";
            TimeSpan refreshTokenExpiration = TimeSpan.FromDays((int)ExpirationTime.REFRESH_TOKEN);

            bool isStored = await _cacheService.StoreAsync(refreshTokenKey, refreshToken, refreshTokenExpiration);

            if(!isStored)
                return Result.Failure("Please login again", HttpStatusCode.InternalServerError);

            SetJWTCookie(jwtToken);

            return Result.Success(refreshToken, HttpStatusCode.OK);
        }

        private string CreateJwtToken(UserDTO user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:secretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expirationTime = Convert.ToInt32(_configuration["Jwt:ExpirationDays"] ?? "7");


            var tokenDiscriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Claims = claims.ToDictionary(c => c.Type, c => (object)c.Value),
                Expires = DateTime.UtcNow.AddDays(expirationTime),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDiscriptor);

            return tokenHandler.WriteToken(token); 
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        private void SetJWTCookie(string jwtToken)
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                MaxAge = TimeSpan.FromMinutes((int)ExpirationTime.JWT_COOKIE),
            };

            _contextAccessor.HttpContext.Response.Cookies.Append("jwt", jwtToken, options);
        }

        public async Task<Result> RefreshTokenAsync(string refreshToken)
        {
            var userId = await _cacheService.GetAsync($"refresh-token:{refreshToken}");

            if (string.IsNullOrEmpty(userId) || Guid.TryParse(userId, out var userGuid))
                return Result.Failure("Refresh token is invalid or expired.", HttpStatusCode.Unauthorized);

            var user = new UserDTO { Id = userGuid };

            string newJwtToken = CreateJwtToken(user);

            SetJWTCookie(newJwtToken);

            return Result.Success(HttpStatusCode.NoContent);
        }
   
        public async Task<bool> StorePasswordResetToken(Guid userId, string token)
        {
            TimeSpan expirationTime = ToTimeSpan.covertToTimeSpan(ExpirationTime.PASSWORD_VERIFICATION, TimeUnitConversion.MINUTES);
            string verificationKey = $"password_reset:{token}";

            return await _cacheService.StoreAsync(verificationKey, userId.ToString(), expirationTime);
        }

        public async Task<string?> GetPasswordResetTokenIfAny(string key)
        {
            string storedKey = $"password_reset_{key}";
            string userId = await _cacheService.GetAsync(storedKey);

            await _cacheService.RemoveAsync(storedKey);

            return userId;
        }

       public async Task RemoveTokensInCache(Guid userId)
        {
            string refreshTokenKey = $"refresh-token:{userId}";
            await _cacheService.RemoveAsync(refreshTokenKey);

        }
    }
}
