namespace TaxCalculator.DAL.Entities
{
    public class TaxCalculation
    {
        public int Id { get; set; }
        public string PostalCode { get; set; }
        public decimal AnnualIncome { get; set; }
        public decimal CalculatedTax { get; set; }
        public DateTime Timestamp { get; set; }
    }
}