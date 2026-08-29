namespace FinTrack.Application.DTOs.Authentication
{
    public class AuthenticationResponse
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }

        public required UserSummaryResponse User { get; set; }


    }
}