using System.Collections;
using WealthFlow.Application.Transactions.DTOs;
using WealthFlow.Shared.Helpers;
using static WealthFlow.Domain.Enums.Enum;

namespace WealthFlow.Application.Transactions.Interfaces
{
    public interface IIncomeService
    {
        public Task<Result<string>> StoreIncomeAsync(IncomeDTO incomeDTO);
        public Task<Result<Object>> UpdateIncomeAsync(IncomeDTO incomeDTO);
        public Task<Result<string>> DeleteBulkOrSingleIncomeDetailsAsync(List<Guid> incomeIds);
        public Task<Result<Object>> GetAllIncomeDetailsAsync(int pageNumber, int pageSize, SortBy sortBy, SortOrderBy orderBy);
        public Task<Result<Object>> GetDateSpecificIncomeDetailsAsync(DateTime sartingDate, DateTime endDate);
        


    }
}
