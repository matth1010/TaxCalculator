namespace TaxCalculator.API.Models
{
    public class TaxCalculationResponse
    {
        public int Id { get; set; }

        public string PostalCode { get; set; }

        public decimal AnnualIncome { get; set; }

        public decimal TaxAmount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}