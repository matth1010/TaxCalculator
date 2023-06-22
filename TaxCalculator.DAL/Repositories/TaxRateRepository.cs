using Microsoft.EntityFrameworkCore;
using TaxCalculator.DAL.Context;
using TaxCalculator.DAL.Interfaces;
using TaxCalculator.DAL.Models;

namespace TaxCalculator.DAL.Repositories
{
    public class TaxRateRepository : ITaxRateRepository
    {
        private readonly TaxCalculatorDbContext _context;

        public TaxRateRepository(TaxCalculatorDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaxRate>> GetAllTaxRatesAsync()
        {
            return await _context.TaxRates.ToListAsync();
        }

        public async Task<TaxRate> GetTaxRateByPostalCodeAsync(string postalCode)
        {
            return await _context.TaxRates.FirstOrDefaultAsync(t => t.PostalCode == postalCode);
        }
    }
}