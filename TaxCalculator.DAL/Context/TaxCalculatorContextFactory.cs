using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TaxCalculator.DAL.Context
{
    public class TaxCalculatorContextFactory : IDesignTimeDbContextFactory<TaxCalculatorDbContext>
    {
        public TaxCalculatorDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TaxCalculatorDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new TaxCalculatorDbContext(optionsBuilder.Options);
        }
    }
}