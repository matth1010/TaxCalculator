using System;
using System.ComponentModel.DataAnnotations;

namespace TaxCalculator.DAL.Models
{
    public class TaxRecord
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string PostalCode { get; set; }

        public decimal AnnualIncome { get; set; }

        public decimal TaxAmount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}