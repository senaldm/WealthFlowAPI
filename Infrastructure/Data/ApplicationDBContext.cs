using Microsoft.EntityFrameworkCore;
using WealthFlow.Domain.Entities;
using WealthFlow.Application.Security.Interfaces;
namespace WealthFlow.Infrastructure.Data
{
    public class ApplicationDBContext : DbContext
    {
        private readonly IPasswordService _passwordService;
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options, IPasswordService passwordService) : base(options)
        {
            _passwordService = passwordService;
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>().HasData(
                new User { Id = Guid.NewGuid(), Name = "Senal Dimuthu", 
                    Email = "wealthflow.pft@gmail.com", Password = _passwordService.HashPassword("admin@123"),
                    Role = "Admin", RecoveryEmail = "silentshadowslr12@gmail.com"}
    );
        }
    }
}
