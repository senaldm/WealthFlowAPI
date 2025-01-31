namespace WealthFlow.Domain.Entities.Transactions
{
    public class IncomeType
    {
        public int IncomeTypeId { get; set; }
        public string TypeName { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual ICollection<Income> Incomes { get; set; } = new List<Income>();
    }
}
