using TaxCalculator.DAL.Models;

namespace TaxCalculator.API.Models
{
    public class TaxRateResponse
    {
        public int Id { get; set; }
        public string PostalCode { get; set; }
        public CalculationType CalculationType { get; set; }
    }
}
