using WealthFlow.Domain.Entities.Transactions;

namespace WealthFlow.Infrastructure.Transactions.Repositories
{
    public interface IIncomeRepository
    {
        public Task<Income?> GetIncomeDetailsAsync(Guid incomeId);
        public Task<bool> UpdateIncomeDetailsAsync(Income income);
        public Task<bool> DeleteIncomeDetailsAsync(Income income);
        public Task<bool> StoreIncomeDetailsAsync(Income income);
        public Task<IEnumerable<Income>> GetAllIncomeDetailsAsync(Guid userId);
        public Task<IEnumerable<Income>> GetDateRangeSpecificIncomeDetailsAsync(DateTime statingDate, DateTime endDate, Guid userId);

    }
}
