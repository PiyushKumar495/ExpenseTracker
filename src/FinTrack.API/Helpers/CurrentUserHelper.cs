using System.Security.Claims;

namespace FinTrack.API.Helpers
{
    public static class CurrentUserHelper
    {
        public static bool TryGetUserId(ClaimsPrincipal user, out Guid userId)
        {
            userId = Guid.Empty;

            var claim = user.FindFirst(ClaimTypes.NameIdentifier);

            return claim != null &&
                   Guid.TryParse(claim.Value, out userId);
        }
    }
}