namespace FinTrack.Application.DTOs.Users
{
    public class UpdateUserRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Currency { get; set; }
        public required string TimeZone { get; set; }


    }
}