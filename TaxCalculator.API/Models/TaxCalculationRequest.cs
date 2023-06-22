using System.ComponentModel.DataAnnotations;

namespace TaxCalculator.API.Models
{
    public class TaxCalculationRequest
    {
        [Required]
        public string PostalCode { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal AnnualIncome { get; set; }
    }
}