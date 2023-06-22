namespace TaxCalculator.Web.ViewModels
{
    public class TaxCalculationViewModel
    {
        public int Id { get; set; }
        public string PostalCode { get; set; }
        public decimal AnnualIncome { get; set; }
        public decimal TaxAmount { get; set; }
    }

}
