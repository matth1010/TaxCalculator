// TaxCalculator.DAL.Models project - TaxCalculation.cs

using System.ComponentModel.DataAnnotations;

namespace TaxCalculator.DAL.Models
{
    public class TaxCalculationInputModel
    {
        [Required]
        public string PostalCode { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal AnnualIncome { get; set; }
    }
}