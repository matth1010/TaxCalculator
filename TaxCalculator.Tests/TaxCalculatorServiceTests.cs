// TaxCalculator.Tests.Services namespace

using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using TaxCalculator.BL.Services;
using TaxCalculator.DAL.Entities;
using TaxCalculator.BL.Interfaces;
using TaxCalculator.DAL.Models;

namespace TaxCalculator.Tests.Services
{
    [TestFixture]
    public class TaxCalculatorServiceTests
    {
        private Mock<ITaxRateRepository> _taxRateRepositoryMock;
        private Mock<ITaxCalculationRepository> _taxCalculationRepositoryMock;
        private TaxCalculatorService _taxCalculatorService;

        [SetUp]
        public void Setup()
        {
            _taxRateRepositoryMock = new Mock<ITaxRateRepository>();
            _taxCalculationRepositoryMock = new Mock<ITaxCalculationRepository>();
            _taxCalculatorService = new TaxCalculatorService();
        }

        [Test]
        public async Task CalculateTaxAsync_ProgressiveRate_ReturnsCorrectTaxAmount()
        {
            // Arrange
            var taxRate = new TaxRate { PostalCode = "7441", CalculationType = CalculationType.Progressive };
            var taxCalculation = new TaxCalculation { PostalCode = "7441", AnnualIncome = 100000 };

            _taxRateRepositoryMock.Setup(r => r.GetTaxRateByPostalCodeAsync(taxCalculation.PostalCode))
                .ReturnsAsync(taxRate);

            // Act
            var calculatedTax = _taxCalculatorService.CalculateTax(taxCalculation.PostalCode, taxCalculation.AnnualIncome);

            // Assert
            // Add your assertions here to verify the calculated tax amount
        }

        // Add more test cases for different scenarios

        [TearDown]
        public void TearDown()
        {
            _taxRateRepositoryMock.Reset();
            _taxCalculationRepositoryMock.Reset();
        }
    }
}
