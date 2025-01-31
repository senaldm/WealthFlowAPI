using WealthFlow.Domain.Entities.User;
using WealthFlow.Application.Security.Interfaces;
using WealthFlow.Infrastructure.Persistence.DBContexts;

namespace WealthFlow.Infrastructure.Persistence.Seaders
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
            if (_dbContext.Users.Any())
            {
                _dbContext.Users.AddRange(new List<User>
                 {
                    new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Senal Dimuthu",
                    Email = "wealthflow.pft@gmail.com",
                    Password = _passwordService.HashPassword("admin@123"),
                    Role = "Admin",
                    RecoveryEmail = "silentshadowslr12@gmail.com"
                }

                });

                _dbContext.SaveChanges();
            }
        }
    }
}
