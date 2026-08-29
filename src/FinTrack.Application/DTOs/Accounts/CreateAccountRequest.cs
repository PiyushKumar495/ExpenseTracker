using FinTrack.Domain.Enums;
namespace FinTrack.Application.DTOs.Accounts
{
    public class CreateAccountRequest
    {
        public required string Name { get; set; }
        public AccountType AccountType { get; set; }
        public decimal OpeningBalance { get; set; }
        public required string Currency { get; set; }
        public string? Description { get; set; }
        

    }
}