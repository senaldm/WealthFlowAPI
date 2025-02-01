using WealthFlow.Domain.Entities.Transactions;
using WealthFlow.Shared.Helpers;
using static WealthFlow.Domain.Enums.Enum;

namespace WealthFlow.Infrastructure.Transactions.Repositories
{
    public interface IExpenseRepository
    {
        public Task<List<Expense>> GetAllExpenseDetailsAsync(Guid userId, int pageNumber, int pageSize, SortBy sort, SortOrderBy order);
        public Task<List<Expense>> GetBulkOfExpenseDetailsAsync(List<Guid> expenseIds);
        public Task<bool> RemoveBulkOrSingleExpenseDetailsAsync(List<Guid> expenseIds);
        public Task<bool> StoreBulkOrSingleExpenseDetailsAsync(List<Expense> expenseDetails);
        public Task<bool> UpdateExpenseDetailsAsync(Expense expense);
        public Task<List<Expense>> GetDateRangeSpecificExpenseDetailsAsync(DateTime statingDate, DateTime endDate, Guid userId);
    }
}
