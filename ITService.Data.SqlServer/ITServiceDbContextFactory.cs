using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace RepairService.Data.SqlServer;

public class RepairServiceDbContextFactory : IDesignTimeDbContextFactory<RepairServiceDbContext>
{
    public RepairServiceDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.database.json")
            .Build();

        return CreateDbContext(configuration);
    }

    public RepairServiceDbContext CreateDbContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<RepairServiceDbContext>();
        optionsBuilder.UseSqlServer(connectionString);


        return new RepairServiceDbContext(optionsBuilder.Options);
    }
}
