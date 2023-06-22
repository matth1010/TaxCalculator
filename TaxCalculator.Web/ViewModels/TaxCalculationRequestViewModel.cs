using System.ComponentModel.DataAnnotations;

namespace TaxCalculator.Web.ViewModels
{
    public class TaxCalculationRequestViewModel
    {
        [Required(ErrorMessage = "Postal Code is required.")]
        [StringLength(10, ErrorMessage = "Postal Code must be at most 10 characters long.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Annual Income is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Annual Income must be a non-negative value.")]
        public decimal AnnualIncome { get; set; }
    }
}
