using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Versus.Data
{
    public class VersusContextFactory : IDesignTimeDbContextFactory<VersusContext>
    {
        public VersusContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>(optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<VersusContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection String 'VersusContext' not found.");

            optionsBuilder.UseNpgsql(connectionString);
            
            return new VersusContext(optionsBuilder.Options);
        }
    }
}
