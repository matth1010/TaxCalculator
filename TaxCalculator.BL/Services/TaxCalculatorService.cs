using System;
using System.Threading.Tasks;
using TaxCalculator.BL.Interfaces;
using TaxCalculator.DAL.Entities;
using TaxCalculator.DAL.Interfaces;
using TaxCalculator.DAL.Models;

namespace TaxCalculator.BL.Services
{
    public class TaxCalculatorService : ITaxCalculatorService
    {
        private readonly ITaxRateRepository _taxRateRepository;
        private readonly ITaxRecordRepository _taxRecordRepository;

        public TaxCalculatorService(ITaxRateRepository taxRateRepository, ITaxRecordRepository taxRecordRepository)
        {
            _taxRateRepository = taxRateRepository;
            _taxRecordRepository = taxRecordRepository;
        }

        public async Task<decimal> CalculateTaxAsync(TaxRecord taxCalculation)
        {
            var taxRate = await _taxRateRepository.GetTaxRateByPostalCodeAsync(taxCalculation.PostalCode);
            decimal calculatedTax = 0;

            if (taxRate is null)
                throw new InvalidOperationException("Tax rate is not available");

            calculatedTax = taxRate.CalculationType switch
            {
                CalculationType.Progressive => CalculateProgressiveTax(taxCalculation.AnnualIncome),
                CalculationType.FlatValue => CalculateFlatValueTax(taxCalculation.AnnualIncome),
                CalculationType.FlatRate => CalculateFlatRateTax(taxCalculation.AnnualIncome),
                _ => throw new InvalidOperationException("Invalid tax calculation type.")
            };

            return calculatedTax;
        }

        private decimal CalculateProgressiveTax(decimal annualIncome)
        {
            decimal taxAmount = annualIncome switch
            {
                <= 8350 => annualIncome * 0.10m,
                <= 33950 => 8350 * 0.10m + (annualIncome - 8350) * 0.15m,
                <= 82250 => 8350 * 0.10m + (33950 - 8350) * 0.15m + (annualIncome - 33950) * 0.25m,
                <= 171550 => 8350 * 0.10m + (33950 - 8350) * 0.15m + (82250 - 33950) * 0.25m + (annualIncome - 82250) * 0.28m,
                <= 372950 => 8350 * 0.10m + (33950 - 8350) * 0.15m + (82250 - 33950) * 0.25m + (171550 - 82250) * 0.28m + (annualIncome - 171550) * 0.33m,
                _ => 8350 * 0.10m + (33950 - 8350) * 0.15m + (82250 - 33950) * 0.25m + (171550 - 82250) * 0.28m + (372950 - 171550) * 0.33m + (annualIncome - 372950) * 0.35m
            };

            return taxAmount;
        }

        private decimal CalculateFlatValueTax(decimal annualIncome)
        {
            decimal taxAmount = annualIncome >= 200000 ? annualIncome * 0.05m : 10000m;
            return taxAmount;
        }

        private decimal CalculateFlatRateTax(decimal annualIncome)
        {
            decimal taxAmount = annualIncome * 0.175m;
            return taxAmount;
        }
    }
}
