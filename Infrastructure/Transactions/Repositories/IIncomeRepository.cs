using MailKit.Search;
using WealthFlow.Domain.Entities.Transactions;
using static WealthFlow.Domain.Enums.Enum;
namespace WealthFlow.Infrastructure.Transactions.Repositories
{
    public interface IIncomeRepository
    {
        public Task<List<Income>> GetBulkOrSingleIncomeDetailsAsync(List<Guid> incomeIds);
        public Task<bool> UpdateIncomeDetailsAsync(Income income);
        public Task<bool> DeleteBulkOrSingleIncomeDetailsAsync(List<Guid> incomeIds);
        public Task<bool> StoreIncomeDetailsAsync(Income income);
        public Task<List<Income>> GetAllIncomeDetailsAsync(Guid userId, int pageNumber, int pageSize, SortBy sort, SortOrderBy order);
        public Task<List<Income>> GetDateRangeSpecificIncomeDetailsAsync(DateTime statingDate, DateTime endDate, Guid userId);

        //public Task<List<Income>> GetSortSpecifiedIncomeDetails()
    }
}
