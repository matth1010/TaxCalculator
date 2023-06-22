namespace TaxCalculator.Web.ViewModels
{
    public class TaxCalculationResponseViewModel
    {
        public string PostalCode { get; set; }
        public decimal AnnualIncome { get; set; }
        public decimal TaxAmount { get; set; }
    }

}
