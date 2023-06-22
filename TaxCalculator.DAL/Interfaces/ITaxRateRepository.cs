using TaxCalculator.DAL.Models;

namespace TaxCalculator.DAL.Interfaces
{
    public interface ITaxRateRepository
    {
        Task<IEnumerable<TaxRate>> GetAllTaxRatesAsync();
        Task<TaxRate> GetTaxRateByPostalCodeAsync(string postalCode);
    }
}