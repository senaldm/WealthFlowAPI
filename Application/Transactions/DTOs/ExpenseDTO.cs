using WealthFlow.Domain.Entities.Transactions;

namespace WealthFlow.Application.Transactions.DTOs
{
    public class ExpenseDTO
    {
        public Guid ExpenseId { get; set; }
        public string ExpenseName { get; set; }
        public string ExpenseDescription { get; set; }
        public decimal ExpenseAmount { get; set; }
        public int PaymentMethodId { get; set; }
        public int ExpenseTypeId { get; set; }
        public DateTime ExpenseDate { get; set; }
    }
}
