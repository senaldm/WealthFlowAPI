using Microsoft.EntityFrameworkCore;
using WealthFlow.Domain.Entities.Transactions;
using WealthFlow.Infrastructure.Persistence.DBContexts;

namespace WealthFlow.Infrastructure.Transactions.Repositories
{
    public class IncomeRepository : IIncomeRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public IncomeRepository(ApplicationDBContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<bool> DeleteIncomeDetailsAsync(Income income)
        {
            _dbContext.Incomes.Remove(income);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Income>> GetAllIncomeDetailsAsync(Guid userId)
        {
            return await _dbContext.Incomes
                .Where(i => i.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Income>> GetDateRangeSpecificIncomeDetailsAsync(DateTime startingDate, DateTime endDate, Guid userId)
        {
            return await _dbContext.Incomes
                .Where(i => i.ReceiveDate >= startingDate && i.ReceiveDate <= endDate && i.UserId == userId)
                .ToListAsync();
        }

        public async Task<Income?> GetIncomeDetailsAsync(Guid incomeId)
        {
            return await _dbContext.Incomes.FindAsync(incomeId);
        }

        public async Task<bool> StoreIncomeDetailsAsync(Income income)
        {
            _dbContext.Incomes.AddAsync(income);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateIncomeDetailsAsync(Income income)
        {
            _dbContext.Incomes.Update(income);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
