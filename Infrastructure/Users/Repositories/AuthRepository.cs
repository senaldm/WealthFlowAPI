using Microsoft.EntityFrameworkCore;
using WealthFlow.Domain.Entities.User;
using WealthFlow.Infrastructure.Persistence.DBContexts;

namespace WealthFlow.Infrastructure.Users.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly ILogger _logger;

        public AuthRepository(ApplicationDBContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> CreateUserAsync(User user)
        {

            await _dbContext.AddAsync(user);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public Task<bool> DeleteVerificationTokenAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveVerificationTokenAsync(string token)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> updatePasswordAsync(User user, string hashedPassword)
        {
            user.Password = hashedPassword;

            return await _dbContext.SaveChangesAsync() > 0;
        }
        public async Task<bool> ValidateEmailAsync(string email)
        {
            var user =  await _dbContext.Users.
                FirstOrDefaultAsync(user => user.Email == email);

            return user == null;
        }

        public Task<bool> ValidateToken(string token, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> verifyCodeAsync(int code)
        {
            throw new NotImplementedException();
        }

        public Task<bool> verifyToken(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
