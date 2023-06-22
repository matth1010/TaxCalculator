using Moq;
using TaxCalculator.BL.Services;
using TaxCalculator.DAL.Interfaces;
using TaxCalculator.DAL.Models;

namespace TaxCalculator.BL.Tests
{
    [TestFixture]
    public class TaxCalculatorServiceTests
    {
        private TaxCalculatorService _taxCalculatorService;
        private Mock<ITaxRateRepository> _taxRateRepositoryMock;
        private Mock<ITaxRecordRepository> _taxRecordRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _taxRateRepositoryMock = new Mock<ITaxRateRepository>();
            _taxRecordRepositoryMock = new Mock<ITaxRecordRepository>();
            _taxCalculatorService = new TaxCalculatorService(_taxRateRepositoryMock.Object, _taxRecordRepositoryMock.Object);
        }

        [Test]
        public async Task CalculateTaxAsync_WhenTaxRateIsNull_ThrowsInvalidOperationException()
        {
            _taxRateRepositoryMock.Setup(repo => repo.GetTaxRateByPostalCodeAsync(It.IsAny<string>()))
                .ReturnsAsync((TaxRate)null);
            var taxRecord = new TaxRecord
            {
                PostalCode = "12345",
                AnnualIncome = 50000m
            };
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _taxCalculatorService.CalculateTaxAsync(taxRecord));
        }

        [Test]
        public async Task CalculateTaxAsync_WhenCalculationTypeIsProgressive_CalculatesProgressiveTax()
        {
            var taxRate = new TaxRate
            {
                CalculationType = CalculationType.Progressive
            };
            _taxRateRepositoryMock.Setup(repo => repo.GetTaxRateByPostalCodeAsync(It.IsAny<string>()))
                .ReturnsAsync(taxRate);
            var taxRecord = new TaxRecord
            {
                PostalCode = "12345",
                AnnualIncome = 50000m
            };

            var result = await _taxCalculatorService.CalculateTaxAsync(taxRecord);

            Assert.That(result, Is.EqualTo(8687.5m));
        }

        [Test]
        public async Task CalculateTaxAsync_WhenCalculationTypeIsFlatValue_CalculatesFlatValueTax()
        {
            var taxRate = new TaxRate
            {
                CalculationType = CalculationType.FlatValue
            };
            _taxRateRepositoryMock.Setup(repo => repo.GetTaxRateByPostalCodeAsync(It.IsAny<string>()))
                .ReturnsAsync(taxRate);
            var taxRecord = new TaxRecord
            {
                PostalCode = "12345",
                AnnualIncome = 250000m
            };

            var result = await _taxCalculatorService.CalculateTaxAsync(taxRecord);

            Assert.That(result, Is.EqualTo(12500m));
        }

        [Test]
        public async Task CalculateTaxAsync_WhenCalculationTypeIsFlatRate_CalculatesFlatRateTax()
        {
            var taxRate = new TaxRate
            {
                CalculationType = CalculationType.FlatRate
            };
            _taxRateRepositoryMock.Setup(repo => repo.GetTaxRateByPostalCodeAsync(It.IsAny<string>()))
                .ReturnsAsync(taxRate);
            var taxRecord = new TaxRecord
            {
                PostalCode = "12345",
                AnnualIncome = 100000m
            };

            var result = await _taxCalculatorService.CalculateTaxAsync(taxRecord);

            Assert.That(result, Is.EqualTo(17500m));
        }
    }
}