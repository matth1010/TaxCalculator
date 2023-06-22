using TaxCalculator.DAL.Models;

namespace TaxCalculator.DAL.Interfaces
{
    public interface ITaxRecordRepository
    {
        Task<IEnumerable<TaxRecord>> GetAllTaxRecordsAsync();
        Task<TaxRecord> GetTaxRecordByIdAsync(int id);
        Task<TaxRecord> AddTaxCalculationAsync(TaxRecord taxRecord);
        Task UpdateTaxRecordAsync(TaxRecord taxRecord);
        Task DeleteTaxRecordAsync(TaxRecord taxRecord);
    }
}