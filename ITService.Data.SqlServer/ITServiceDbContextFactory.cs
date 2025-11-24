using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ITService.Data.SqlServer
{
    public class ITServiceDbContextFactory : IDesignTimeDbContextFactory<ITServiceDbContext>
    {
        public ITServiceDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.database.json")
                .Build();

            return CreateDbContext(configuration);
        }

        // Добавлен отдельный метод для использования в UI
        public ITServiceDbContext CreateDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ITServiceDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ITServiceDbContext(optionsBuilder.Options);
        }
    }
}
