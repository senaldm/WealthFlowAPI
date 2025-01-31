using System.Collections;
using WealthFlow.Application.Transactions.DTOs;
using WealthFlow.Shared.Helpers;

namespace WealthFlow.Application.Transactions.Interfaces
{
    public interface IIncomeService
    {
        public Task<Result<string>> StoreIncomeAsync(IncomeDTO incomeDTO);
        public Task<Result<Object>> UpdateIncomeAsync(IncomeDTO incomeDTO);
        public Task<Result<string>> DeleteIncomeAsync(Guid IncomeId);
        public Task<Result<Object>> GetAllIncomeDetailsAsync();
        public Task<Result<Object>> GetDateSpeciftcIncomeDetailsAsync(DateTime sartingDate, DateTime endDate);
        


    }
}
