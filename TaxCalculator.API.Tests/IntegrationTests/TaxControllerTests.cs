using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TaxCalculator.API.Controllers;
using TaxCalculator.API.Models;
using TaxCalculator.BL.Interfaces;
using TaxCalculator.DAL.Interfaces;
using TaxCalculator.DAL.Models;

namespace TaxCalculator.API.Tests.Controllers
{
    [TestFixture]
    public class TaxControllerTests
    {
        private Mock<ITaxRecordRepository> _taxRecordRepositoryMock;
        private Mock<ITaxRateRepository> _taxRateRepositoryMock;
        private Mock<ITaxCalculatorService> _taxCalculatorServiceMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<TaxController>> _loggerMock;
        private TaxController _taxController;

        [SetUp]
        public void Setup()
        {
            _taxRecordRepositoryMock = new Mock<ITaxRecordRepository>();
            _taxCalculatorServiceMock = new Mock<ITaxCalculatorService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<TaxController>>();
            _taxController = new TaxController(_taxCalculatorServiceMock.Object, _taxRecordRepositoryMock.Object, _taxRateRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task CalculateTax_WhenModelStateIsInvalid_ReturnsBadRequest()
        {
            _taxController.ModelState.AddModelError("PostalCode", "The PostalCode field is required.");
            var request = new TaxCalculationRequest();

            var result = await _taxController.CalculateTax(request);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task CalculateTax_WhenCalculationIsSuccessful_ReturnsOkWithResponse()
        {
            var taxRecord = new TaxRecord { PostalCode = "12345", AnnualIncome = 50000m };
            var taxCalculationRequest = new TaxCalculationRequest();
            var taxCalculationResponse = new TaxCalculationResponse();
            _mapperMock.Setup(mapper => mapper.Map<TaxRecord>(taxCalculationRequest))
                .Returns(taxRecord);
            _taxCalculatorServiceMock.Setup(service => service.CalculateTaxAsync(taxRecord))
                .ReturnsAsync(8687.5m);
            _taxRecordRepositoryMock.Setup(repo => repo.AddTaxCalculationAsync(taxRecord))
                .ReturnsAsync(taxRecord);
            _mapperMock.Setup(mapper => mapper.Map<TaxCalculationResponse>(taxRecord))
                .Returns(taxCalculationResponse);

            var result = await _taxController.CalculateTax(taxCalculationRequest);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(taxCalculationResponse, okResult.Value);
        }

        [Test]
        public async Task GetTaxRecord_WhenRecordDoesNotExist_ReturnsNotFound()
        {
            int id = 1;
            _taxRecordRepositoryMock.Setup(repo => repo.GetTaxRecordByIdAsync(id))
                .ReturnsAsync((TaxRecord)null);

            var result = await _taxController.GetTaxRecord(id);

            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task UpdateTaxRecord_WhenRecordExists_ReturnsOkWithUpdatedRecord()
        {
            int id = 1;
            var existingRecord = new TaxRecord { Id = id };
            var request = new TaxCalculationRequest();
            var updatedRecord = new TaxRecord { Id = id, TaxAmount = 1000m };
            var taxCalculationResponse = new TaxCalculationResponse { Id = id, TaxAmount = 1000m };
            _taxRecordRepositoryMock.Setup(repo => repo.GetTaxRecordByIdAsync(id))
                .ReturnsAsync(existingRecord);
            _mapperMock.Setup(mapper => mapper.Map(request, existingRecord));
            _taxCalculatorServiceMock.Setup(service => service.CalculateTaxAsync(existingRecord))
                .ReturnsAsync(updatedRecord.TaxAmount);
            _taxRecordRepositoryMock.Setup(repo => repo.UpdateTaxRecordAsync(existingRecord))
                .Returns(Task.CompletedTask);
            _mapperMock.Setup(mapper => mapper.Map<TaxCalculationResponse>(existingRecord))
                .Returns(taxCalculationResponse);

            var result = await _taxController.UpdateTaxRecord(id, request);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(taxCalculationResponse, okResult.Value);
        }

        [Test]
        public async Task UpdateTaxRecord_WhenRecordDoesNotExist_ReturnsNotFound()
        {
            int id = 1;
            _taxRecordRepositoryMock.Setup(repo => repo.GetTaxRecordByIdAsync(id))
                .ReturnsAsync((TaxRecord)null);

            var result = await _taxController.UpdateTaxRecord(id, new TaxCalculationRequest());

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DeleteTaxRecord_WhenRecordExists_ReturnsOkWithSuccessMessage()
        {
            // Arrange
            int id = 1;
            var taxRecord = new TaxRecord { Id = id };
            _taxRecordRepositoryMock.Setup(repo => repo.GetTaxRecordByIdAsync(id))
                .ReturnsAsync(taxRecord);
            _taxRecordRepositoryMock.Setup(repo => repo.DeleteTaxRecordAsync(taxRecord))
                .Returns(Task.CompletedTask);

            var result = await _taxController.DeleteTaxRecord(id);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual("Tax record deleted successfully.", okResult.Value);
        }

        [Test]
        public async Task DeleteTaxRecord_WhenRecordDoesNotExist_ReturnsNotFound()
        {
            int id = 1;
            _taxRecordRepositoryMock.Setup(repo => repo.GetTaxRecordByIdAsync(id))
                .ReturnsAsync((TaxRecord)null);

            var result = await _taxController.DeleteTaxRecord(id);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

    }
}