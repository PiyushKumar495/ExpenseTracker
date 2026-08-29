using FinTrack.Domain.Enums;
namespace FinTrack.Application.DTOs.Transactions
{
    public class CreateTransferRequest
    {
        public Guid FromAccountId { get; set; }
        public Guid? ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}