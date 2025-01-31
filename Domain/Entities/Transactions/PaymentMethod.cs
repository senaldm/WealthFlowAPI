namespace WealthFlow.Domain.Entities.Transactions
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }
        public string MethodName { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual ICollection<Income> Incomes { get; set; } = new List<Income>();
        public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    }
}
