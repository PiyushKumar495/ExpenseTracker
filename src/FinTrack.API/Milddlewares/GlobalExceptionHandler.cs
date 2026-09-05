using Microsoft.AspNetCore.Diagnostics;

namespace FinTrack.API.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
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