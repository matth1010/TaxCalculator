using Microsoft.AspNetCore.Mvc;
using TaxCalculator.API.Shared;

namespace TaxCalculator.API.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ILogger<BaseController> _logger;

        protected BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }

        protected virtual async Task<ActionResult> TryAsyncCatch(string actionInProgress, Func<Task<ActionResult>> tryBlock, Func<Exception, ActionResult> catchBlock = null, params Func<ActionResult>[] validators)
        {
            ActionResult validationResult = PerformValidation($" while {actionInProgress}", validators);
            if (validationResult != null)
            {
                return validationResult;
            }

            try
            {
                _logger.LogDebug($"BEGIN: {actionInProgress}");
                ActionResult result = await tryBlock();
                _logger.LogDebug($"COMPLETE: {actionInProgress}");
                return result;
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Reason = ex.Message });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { Reason = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Reason = ex.Message });
            }
            catch (Exception ex)
            {
                return catchBlock?.Invoke(ex) ?? ExceptionThrown(ex, actionInProgress, includeMessagesInDetails: false);
            }
        }

        protected virtual ActionResult TryCatch(string actionInProgress, Func<ActionResult> tryBlock, Func<Exception, ActionResult> catchBlock = null, params Func<ActionResult>[] validators)
        {
            return TryAsyncCatch(actionInProgress, () => Task.FromResult(tryBlock()), catchBlock, validators).Result;
        }

        protected IEnumerable<T> FilterEnumerable<T>(IEnumerable<T> toBeFiltered, Predicate<T> keepThisOne)
        {
            return toBeFiltered.Where(item => keepThisOne(item));
        }

        protected ObjectResult StatusCode(int statusCode, string value)
        {
            return StatusCode(statusCode, CreateReasonResponse(value));
        }

        protected object CreateReasonResponse(string reason)
        {
            return new { Reason = reason };
        }

        protected virtual ActionResult ExceptionThrown(Exception ex, string actionBeingPerformed, bool includeMessagesInDetails = true, int statusCode = 500)
        {
            string errorMessage = $"An error occurred while {actionBeingPerformed}";
            LogExceptionMessage(ex, errorMessage);
            List<string> exceptionMessages = includeMessagesInDetails ? ReadExceptionMessages(ex) : new List<string> { "Refer to logs for further details." };

            return StatusCode(statusCode, new
            {
                Reason = errorMessage,
                Details = exceptionMessages.Where(d => !string.IsNullOrWhiteSpace(d)).ToList()
            });
        }

        private void LogExceptionMessage(Exception ex, string message)
        {
            _logger.LogError(ex, message);
        }

        protected virtual ActionResult Forbidden(string reason = null)
        {
            return StatusCode(403, new { Reason = reason });
        }

        protected ActionResult Forbidden(object value)
        {
            return StatusCode(403, value);
        }

        private List<string> ReadExceptionMessages(Exception ex)
        {
            var exceptionMessages = new List<string>();

            for (Exception currentException = ex; currentException != null; currentException = currentException.InnerException)
            {
                exceptionMessages.Add(currentException.Message);
            }

            return exceptionMessages;
        }

        protected ActionResult PerformValidation(string logContext, IEnumerable<Func<ActionResult>> validators)
        {
            int num = 0;

            foreach (Func<ActionResult> validator in validators ?? Enumerable.Empty<Func<ActionResult>>())
            {
                _logger.LogDebug($"Performing [{++num}] validator {logContext}");
                ActionResult actionResult = validator();

                if (actionResult != null)
                {
                    return actionResult;
                }
            }

            return null;
        }
    }
}