using TaxCalculator.DAL.Entities;
using TaxCalculator.DAL.Models;

namespace TaxCalculator.BL.Interfaces
{
    public interface ITaxCalculatorService
    {
        Task<decimal> CalculateTaxAsync(TaxRecord taxCalculation);
    }
}