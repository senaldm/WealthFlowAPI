using WealthFlow.Application.Users.Interfaces;
using WealthFlow.Infrastructure.Data;
using WealthFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WealthFlow.Infrastructure.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public UserRepository(ApplicationDBContext dbContext) 
        {
            _dbContext = dbContext; 
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = GetUserByIdAsync(userId);

            if (user == null)
                return false;

            _dbContext.Remove(user);
            return await _dbContext.SaveChangesAsync() > 0;
                
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _dbContext.Users.FindAsync(id);
        }
        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(user =>user.Email == email);

            return user == null;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _dbContext.Users.Update(user);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<string?> GetUserEmailUsingRecoveryEmail(string recoveryEmail)
        {
            return await _dbContext.Users
                .Where(user => user.RecoveryEmail == recoveryEmail || user.Email == recoveryEmail)
                .Select(user => user.Email)
                .FirstOrDefaultAsync();
        }
    }
}
