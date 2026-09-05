using Microsoft.AspNetCore.Http;
namespace FinTrack.API.Models
{
    public static class ErrorMapping
    {
        public static int GetStatusCode(string errorCode)
        {
            return errorCode switch
            {
                "EMAIL_ALREADY_EXISTS" => StatusCodes.Status409Conflict,

                "INVALID_CREDENTIALS" => StatusCodes.Status401Unauthorized,

                "USER_INACTIVE" => StatusCodes.Status403Forbidden,

                "INVALID_REFRESH_TOKEN" => StatusCodes.Status401Unauthorized,

                "REFRESH_TOKEN_EXPIRED" => StatusCodes.Status401Unauthorized,

                "REFRESH_TOKEN_REVOKED" => StatusCodes.Status401Unauthorized,

                _ => StatusCodes.Status400BadRequest
            };
        }
    }
}