using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
namespace FinTrack.API.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is DbUpdateConcurrencyException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

                var concurrencyResponse = new
                {
                    statusCode = StatusCodes.Status409Conflict,
                    code = "CONCURRENCY_CONFLICT",
                    message = "The account was modified by another request. Please retry."
                };

                await httpContext.Response.WriteAsJsonAsync(
                    concurrencyResponse,
                    cancellationToken);

                return true;
            }
            _logger.LogError(exception,"Unhandled exception occurred while processing the request.");
            var response = new
            {
                statusCode = StatusCodes.Status500InternalServerError,
                code = "INTERNAL_SERVER_ERROR",
                message = "An unexpected error occurred."
            };

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(
                response,
                cancellationToken);

            return true;
        }
    }
}