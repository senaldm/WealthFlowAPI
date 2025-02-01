using Microsoft.EntityFrameworkCore;
using WealthFlow.Domain.Entities.Transactions;
using WealthFlow.Infrastructure.Persistence.DBContexts;
using static WealthFlow.Domain.Enums.Enum;

namespace WealthFlow.Infrastructure.Transactions.Repositories
{
    public class IncomeRepository : IIncomeRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public IncomeRepository(ApplicationDBContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<bool> DeleteBulkOrSingleIncomeDetailsAsync(List<Guid> incomeIds)
        {
            var rowAffect = await _dbContext.Incomes
                .Where(i => incomeIds.Contains(i.IncomeId))
                .ExecuteDeleteAsync();

            return rowAffect > 0;
        }

        public async Task<List<Income>> GetAllIncomeDetailsAsync(Guid userId, int pageNumber, int pageSize, SortBy sort, SortOrderBy order)
        {
            var query = _dbContext.Incomes
                .Where(i => i.UserId == userId)
                .AsQueryable();

            if (sort == SortBy.CREATED_DATE && order == SortOrderBy.DESC)
                query = query.OrderByDescending(i => i.CreatedAt);

            else if (sort == SortBy.CREATED_DATE && order == SortOrderBy.ASC)
                query = query.OrderBy(i => i.CreatedAt);

            else if (sort == SortBy.VALUE && order == SortOrderBy.DESC)
                query = query.OrderByDescending(i => i.IncomeAmount);

            else if (sort == SortBy.VALUE && order == SortOrderBy.ASC)
                query = query.OrderBy(i => i.IncomeAmount);

            return await query
                         .Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize)
                         .ToListAsync();
        }

        public async Task<List<Income>> GetDateRangeSpecificIncomeDetailsAsync(DateTime startingDate, DateTime endDate, Guid userId)
        {
            return await _dbContext.Incomes
                .Where(i => i.ReceiveDate >= startingDate && i.ReceiveDate <= endDate && i.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Income>> GetBulkOrSingleIncomeDetailsAsync(List<Guid> incomeIds)
        {
            return await _dbContext.Incomes
                .Where(i => incomeIds.Contains(i.IncomeId))
                .ToListAsync();
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
