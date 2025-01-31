namespace WealthFlow.Domain.Entities.Transactions
{
    public class ExpenseType
    {
        public int ExpenceTypeId { get; set; }
        public string ExpenceName { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual ICollection<Expense> Expences { get; set; } = new List<Expense>();
    }
}
