using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WealthFlow.Application.Security.Interfaces;
using WealthFlow.Application.Security.Services;
using WealthFlow.Domain.Entities.Transactions;
using WealthFlow.Domain.Entities.Users;
using WealthFlow.Infrastructure.Persistence.Seeders;
using Microsoft.AspNetCore.Identity;
namespace WealthFlow.Infrastructure.Persistence.DBContexts
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ExpenseType> ExpenseTypes { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<IncomeType> IncomeTypes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<User>(entity =>
            //{
            //    entity.HasKey(u => u.Id);
            //    entity.Property(u => u.Id)
            //          .HasColumnType("CHAR(36)") 
            //          .HasDefaultValueSql("(UUID())"); 
            //});

            modelBuilder.Entity<ExpenseType>(entity =>
            {
                entity.HasKey(et => et.ExpenceTypeId);
                entity.Property(et => et.ExpenceTypeId)
                      .ValueGeneratedNever(); // Disable auto-generation
            });

            modelBuilder.Entity<IncomeType>(entity =>
            {
                entity.HasKey(it => it.IncomeTypeId);
                entity.Property(it => it.IncomeTypeId)
                      .ValueGeneratedNever();
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.HasKey(pm => pm.PaymentMethodId);
                entity.Property(pm => pm.PaymentMethodId)
                      .ValueGeneratedNever();
            });

            modelBuilder.Entity<Expense>(entity =>
            {
                entity.HasKey(e => e.ExpenseId);


                entity.HasOne(e => e.User)
                      .WithMany(u => u.Expenses)  // Inverse navigation in User
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ExpenseType)
                      .WithMany(et => et.Expences) // Inverse navigation in ExpenseType
                      .HasForeignKey(e => e.ExpenseTypeId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.PaymentMethod)
                      .WithMany(pm => pm.Expenses) // Inverse navigation in PaymentMethod
                      .HasForeignKey(e => e.PaymentMethodId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Income>(entity =>
            {
                entity.HasKey(e => e.IncomeId);


                entity.HasOne(i => i.User)
                      .WithMany(u => u.Incomes)  // Inverse navigation in User
                      .HasForeignKey(i => i.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(i => i.IncomeType)
                      .WithMany(it =>it.Incomes) // Inverse navigation in ExpenseType
                      .HasForeignKey(i => i.IncomeTypeId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.PaymentMethod)
                      .WithMany(pm => pm.Incomes) // Inverse navigation in PaymentMethod
                      .HasForeignKey(e => e.PaymentMethodId)
                      .OnDelete(DeleteBehavior.Cascade);
            });


        }
    }
}
