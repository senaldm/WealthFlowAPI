using Microsoft.EntityFrameworkCore;
using WealthFlow.Application.Security.Interfaces;
using WealthFlow.Domain.Entities.Transactions;
using WealthFlow.Domain.Entities.User;
namespace WealthFlow.Infrastructure.Persistence.DBContexts
{
    public class ApplicationDBContext : DbContext
    {
        private readonly IPasswordService _passwordService;
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options, IPasswordService passwordService) : base(options)
        {
            _passwordService = passwordService;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ExpenseType> ExpenseTypes { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<IncomeType> IncomeTypes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Senal Dimuthu",
                    Email = "wealthflow.pft@gmail.com",
                    Password = _passwordService.HashPassword("admin@123"),
                    Role = "Admin",
                    RecoveryEmail = "silentshadowslr12@gmail.com"
                }
    );
        }
    }
}
