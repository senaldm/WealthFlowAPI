using WealthFlow.Application.Transactions.DTOs;
using WealthFlow.Shared.Helpers;
using static WealthFlow.Domain.Enums.Enum;

namespace WealthFlow.Application.Transactions.Interfaces
{
    public interface IExpenseService
    {
        public Task<Result<string>> StoreBulkOrSingleExpenseDetailsAsync(List<ExpenseDTO> expenseDTO);
        public Task<Result<Object>> UpdateExpenseAsync(ExpenseDTO expenseDTO);
        public Task<Result<string>> DeleteBulkOrSingleExpenseDetailsAsync(List<Guid> expenseIds);
        public Task<Result<Object>> GetAllExpenseDetailsAsync(int pageNumber, int pageSize, SortBy sort, SortOrderBy order);
        public Task<Result<Object>> GetDateSpecificExpenseDetailsAsync(DateTime startingDate, DateTime endDate);
    }
}
