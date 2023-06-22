using Microsoft.EntityFrameworkCore;
using TaxCalculator.DAL.Models;

namespace TaxCalculator.DAL.Context
{
    public class TaxCalculatorDbContext : DbContext
    {
        public DbSet<TaxRecord> TaxRecords { get; set; }
        public DbSet<TaxRate> TaxRates { get; set; }

        public TaxCalculatorDbContext(DbContextOptions<TaxCalculatorDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaxRecord>().ToTable("TaxRecords");
            modelBuilder.Entity<TaxRate>().ToTable("TaxRates");
        }
    }
}