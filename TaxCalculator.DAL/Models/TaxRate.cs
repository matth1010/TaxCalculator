// TaxCalculator.DAL.Models project - TaxRate.cs

namespace TaxCalculator.DAL.Models
{
    public class TaxRate
    {
        public int Id { get; set; }
        public string PostalCode { get; set; }
        public CalculationType CalculationType { get; set; }
    }

    public enum CalculationType
    {
        Progressive,
        FlatValue,
        FlatRate
    }
}