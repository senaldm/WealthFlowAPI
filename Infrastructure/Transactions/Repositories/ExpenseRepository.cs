using Microsoft.EntityFrameworkCore;
using WealthFlow.Domain.Entities.Transactions;
using WealthFlow.Infrastructure.Persistence.DBContexts;
using static WealthFlow.Domain.Enums.Enum;

namespace WealthFlow.Infrastructure.Transactions.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public ExpenseRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Expense>> GetAllExpenseDetailsAsync(Guid userId, int pageNumber, int pageSize, SortBy sort, SortOrderBy order)
        {
            var query = _dbContext.Expenses
                .Where(i => i.UserId == userId)
                .AsQueryable();

            if (sort == SortBy.CREATED_DATE && order == SortOrderBy.DESC)
                query = query.OrderByDescending(i => i.CreatedAt);

            else if (sort == SortBy.CREATED_DATE && order == SortOrderBy.ASC)
                query = query.OrderBy(i => i.CreatedAt);

            else if (sort == SortBy.VALUE && order == SortOrderBy.DESC)
                query = query.OrderByDescending(i => i.ExpenseAmount);

            else if (sort == SortBy.VALUE && order == SortOrderBy.ASC)
                query = query.OrderBy(i => i.ExpenseAmount);

            return await query
                         .Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize)
                         .ToListAsync();
        
        }

        public async Task<List<Expense>> GetBulkOfExpenseDetailsAsync(List<Guid> expenseIds)
        {
            return await _dbContext.Expenses
                .Where(e=>expenseIds.Contains(e.ExpenseId))
                .ToListAsync();
        }

        public async Task<List<Expense>> GetDateRangeSpecificExpenseDetailsAsync(DateTime statingDate, DateTime endDate, Guid userId)
        {
            return await _dbContext.Expenses
                .Where(e=>e.ExpenseDate >= statingDate && e.ExpenseDate <= endDate && e.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> RemoveBulkOrSingleExpenseDetailsAsync(List<Guid> expenseIds)
        {
           var rowAffected = await _dbContext.Expenses
                   .Where(e => expenseIds.Contains(e.ExpenseId)).ExecuteDeleteAsync();

            return rowAffected > 0;
                
        }

        public async Task<bool> StoreBulkOrSingleExpenseDetailsAsync(List<Expense> expenseDetails)
        {
            await _dbContext.Expenses.AddRangeAsync(expenseDetails);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateExpenseDetailsAsync(Expense expense)
        {
            _dbContext.Expenses.Update(expense);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
