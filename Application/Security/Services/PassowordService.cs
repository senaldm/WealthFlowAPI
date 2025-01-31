﻿using WealthFlow.Application.Security.Interfaces;
using BCrypt.Net;
using System.Security.Cryptography;
using System.Net;
using WealthFlow.Shared.Helpers;
using WealthFlow.Domain.Entities.User;
using WealthFlow.Infrastructure.Users.Repositories;

namespace WealthFlow.Application.Security.Services
{
    public class PassowordService : IPasswordService
    {
        private readonly IAuthRepository _authRepository;

        public PassowordService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;

            if (!password.Any(char.IsLower))
                return false;

            if (!password.Any(char.IsUpper))
                return false;

            if (!password.Any(char.IsDigit))
                return false;

            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                return false;

            return true;
        }

        public  string GeneratePasswordResetToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }

        public async Task<Result> UpdatePasswordIfValidatedAsync(User user,string password)
        {
            if (!IsValidPassword(password))
                return Result.Failure("Password does not meet verification requerements.", HttpStatusCode.BadRequest);
            var hashedPassword = HashPassword(password);

            var success = await _authRepository.updatePasswordAsync(user, hashedPassword);

            if (!success)
                return Result.Failure("Failed to update Password", HttpStatusCode.InternalServerError);

            return Result.Success(HttpStatusCode.OK);
        }

    }
}
