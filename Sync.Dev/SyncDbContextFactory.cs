using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Sync.Common.ConfigurationSettings;
using Sync.DAL;

namespace Sync.Dev
{
    public class SyncDbContextFactory : IDesignTimeDbContextFactory<SyncDbContext>
    {
        public SyncDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SyncDbContext>();
            var configuration = new ConfigurationBuilder()
           .RegisterConfiguration<SyncDbContextFactory>()
           .Build();
            Console.WriteLine(configuration.GetSection("SQLConnectionString").GetSection("ConnectionString").Value);
            optionsBuilder.UseSqlServer(configuration.GetSection("SQLConnectionString").GetSection("ConnectionString").Value, option => option.MigrationsAssembly("Sync.DAL"));

            return new SyncDbContext(optionsBuilder.Options);
        }
    }
}
