using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaxCalculator.API.Constants;
using TaxCalculator.API.Models;
using TaxCalculator.API.Shared;
using TaxCalculator.BL.Interfaces;
using TaxCalculator.DAL.Interfaces;
using TaxCalculator.DAL.Models;
using TaxCalculator.DAL.Repositories;

namespace TaxCalculator.API.Controllers
{
    [ApiController]
    [Route("api/tax")]
    public class TaxController : BaseController
    {
        private readonly ITaxRecordRepository _taxRecordRepository;
        private readonly ITaxRateRepository _taxRateRepository;
        private readonly ITaxCalculatorService _taxCalculatorService;
        private readonly IMapper _mapper;

        public TaxController(ITaxCalculatorService taxCalculatorService, ITaxRecordRepository taxRecordRepository, ITaxRateRepository taxRateRepository, IMapper mapper, ILogger<TaxController> logger) : base(logger)
        {
            _taxCalculatorService = taxCalculatorService;
            _taxRecordRepository = taxRecordRepository;
            _taxRateRepository = taxRateRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Calculates the tax amount based on the provided tax calculation request.
        /// </summary>
        /// <param name="request">The tax calculation request.</param>
        /// <returns>The tax calculation result.</returns>
        [HttpPost("calculate")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TaxCalculationResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(OperationId = nameof(CalculateTax))]
        public async Task<IActionResult> CalculateTax(TaxCalculationRequest request)
        {
            return await TryAsyncCatch("calculating tax", async () =>
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var taxRecord = _mapper.Map<TaxRecord>(request);
                taxRecord.TaxAmount = await _taxCalculatorService.CalculateTaxAsync(taxRecord);
                var addedRecord = await _taxRecordRepository.AddTaxCalculationAsync(taxRecord);

                var response = _mapper.Map<TaxCalculationResponse>(addedRecord);

                return Ok(response);
            },
            ex =>
            {
                if (ex is NotFoundException)
                {
                    return NotFound(new { Reason = ex.Message });
                }
                else if (ex is BadRequestException)
                {
                    return BadRequest(new { Reason = ex.Message });
                }

                return ExceptionThrown(ex, "calculating tax");
            });
        }

        /// <summary>
        /// Retrieves all tax records.
        /// </summary>
        /// <returns>The list of tax records.</returns>
        [HttpGet("records")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<TaxCalculationResponse>))]
        public async Task<ActionResult<IEnumerable<TaxCalculationResponse>>> GetTaxRecords()
        {
            return await TryAsyncCatch("retrieving tax records", async () =>
            {
                var taxRecords = await _taxRecordRepository.GetAllTaxRecordsAsync();
                var response = _mapper.Map<IEnumerable<TaxCalculationResponse>>(taxRecords);

                return Ok(response);
            },
            ex => ExceptionThrown(ex, "retrieving tax records"));
        }

        /// <summary>
        /// Retrieves a specific tax record by its ID.
        /// </summary>
        /// <param name="id">The ID of the tax record to retrieve.</param>
        /// <returns>The tax record with the specified ID.</returns>
        [HttpGet("records/{id}")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TaxCalculationResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaxCalculationResponse>> GetTaxRecord(int id)
        {
            return await TryAsyncCatch("retrieving tax record", async () =>
            {
                var taxRecord = await _taxRecordRepository.GetTaxRecordByIdAsync(id);
                if (taxRecord == null)
                {
                    return NotFound();
                }

                var response = _mapper.Map<TaxCalculationResponse>(taxRecord);

                return Ok(response);
            },
            ex => ExceptionThrown(ex, "retrieving tax record"));
        }

        /// <summary>
        /// Updates an existing tax record.
        /// </summary>
        /// <param name="id">The ID of the tax record to update.</param>
        /// <param name="model">The updated tax record model.</param>
        /// <returns>No content if the tax record was successfully updated.</returns>
        [HttpPut("records/{id}")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TaxCalculationResponse))]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTaxRecord(int id, [FromBody] TaxCalculationRequest request)
        {
            return await TryAsyncCatch("updating tax record", async () =>
            {
                var existingRecord = await _taxRecordRepository.GetTaxRecordByIdAsync(id);
                if (existingRecord == null)
                {
                    return NotFound();
                }

                _mapper.Map(request, existingRecord);

                decimal taxAmount = await _taxCalculatorService.CalculateTaxAsync(existingRecord);
                existingRecord.TaxAmount = taxAmount;

                await _taxRecordRepository.UpdateTaxRecordAsync(existingRecord);

                var response = _mapper.Map<TaxCalculationResponse>(existingRecord);

                return Ok(response);
            },
            ex => ExceptionThrown(ex, "updating tax record"));
        }


        /// <summary>
        /// Deletes a tax record by its ID.
        /// </summary>
        /// <param name="id">The ID of the tax record to delete.</param>
        /// <returns>A message indicating the success or failure of the deletion.</returns>
        [HttpDelete("records/{id}")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(string))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTaxRecord(int id)
        {
            return await TryAsyncCatch("deleting tax record", async () =>
            {
                var taxRecord = await _taxRecordRepository.GetTaxRecordByIdAsync(id);
                if (taxRecord == null)
                {
                    return NotFound();
                }

                await _taxRecordRepository.DeleteTaxRecordAsync(taxRecord);

                return Ok("Tax record deleted successfully.");
            },
            ex => ExceptionThrown(ex, "deleting tax record"));
        }

        /// <summary>
        /// Retrieves all tax rates.
        /// </summary>
        /// <returns>The list of tax rates.</returns>
        [HttpGet("taxrates")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<TaxRateResponse>))]
        public async Task<ActionResult<IEnumerable<TaxRateResponse>>> GetTaxRates()
        {
            return await TryAsyncCatch("retrieving tax rates", async () =>
            {
                var taxRates = await _taxRateRepository.GetAllTaxRatesAsync();

                var response = _mapper.Map<IEnumerable<TaxRateResponse>>(taxRates);

                return Ok(response);
            },
            ex => ExceptionThrown(ex, "retrieving tax rates"));
        }
    }
}