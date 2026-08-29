namespace FinTrack.Application.DTOs.Accounts
{
    public class UpdateAccountRequest
    {
        public required string Name { get; set; }
        public required string Currency { get; set; }
        public string? Description { get; set; }
        

    }
}