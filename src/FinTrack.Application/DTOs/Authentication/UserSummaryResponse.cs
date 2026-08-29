namespace FinTrack.Application.DTOs.Authentication
{
    public class UserSummaryResponse
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Currency { get; set; }
        public required string TimeZone { get; set; }


    }
}