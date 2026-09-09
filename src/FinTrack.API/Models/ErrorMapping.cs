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
                
                "ACCOUNT_NOT_FOUND" => StatusCodes.Status404NotFound,

                "CATEGORY_NOT_FOUND"=> StatusCodes.Status404NotFound,
                
                "PARENT_CATEGORY_NOT_FOUND" => StatusCodes.Status404NotFound,
                
                "SYSTEM_CATEGORY_MODIFICATION_NOT_ALLOWED"=>StatusCodes.Status403Forbidden,
                
                "INVALID_PARENT_CATEGORY"=>StatusCodes.Status400BadRequest,
                
                "TRANSACTION_NOT_FOUND"=>StatusCodes.Status404NotFound,

                "INVALID_TRANSFER" => StatusCodes.Status400BadRequest,
                
                "INVALID_TRANSFER_AMOUNT" => StatusCodes.Status400BadRequest,
                
                "INSUFFICIENT_BALANCE" => StatusCodes.Status400BadRequest,

                "TRANSFER_MODIFICATION_NOT_ALLOWED"=>StatusCodes.Status400BadRequest,
                
                _ => StatusCodes.Status400BadRequest
            };
        }
    }
}