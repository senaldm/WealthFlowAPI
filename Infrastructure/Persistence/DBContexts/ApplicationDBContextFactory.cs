// WealthFlow.Infrastructure/Persistence/DBContexts/ApplicationDBContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WealthFlow.Application.Security.Interfaces;

namespace WealthFlow.Infrastructure.Persistence.DBContexts;

public class ApplicationDBContextFactory : IDesignTimeDbContextFactory<ApplicationDBContext>
{

    public ApplicationDBContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDBContext>();
        optionsBuilder.UseMySql(
            "Server=localhost;Port=3306;Database=wealthflowdb;User=SENAL;Password=Shadowslr1205@;",
            ServerVersion.Parse("8.4.4-mysql")
        );
        return new ApplicationDBContext(optionsBuilder.Options);
    }
}