using WealthFlow.Domain.Entities.Users;
using WealthFlow.Application.Security.Interfaces;
using WealthFlow.Infrastructure.Persistence.DBContexts;

namespace WealthFlow.Infrastructure.Persistence.Seeders
{
    public class AdminDataSeeder
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IPasswordService _passwordService;

        public AdminDataSeeder(ApplicationDBContext dbContext, IPasswordService passwordService)
        {
            _dbContext = dbContext;
            _passwordService = passwordService;
        }

        public void Seed()
        {
            if (!_dbContext.Users.Any(u => u.Email == "wealthflow.pft@gmail.com"))
            {

                var admin = new User
                {
                    FirstName = "Senal",
                    LastName = "Dimuthu",
                    Email = "wealthflow.pft@gmail.com",
                    Password = _passwordService.HashPassword("admin@123"),
                    Role = "Admin",
                    RecoveryEmail = "silentshadowslr12@gmail.com"
                };

                
                _dbContext.Users.Add(admin);

                _dbContext.SaveChanges();
            }
        }
    }
}
