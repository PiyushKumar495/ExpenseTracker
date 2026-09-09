using FinTrack.Domain.Enums;
namespace FinTrack.Application.DTOs.Transactions
{
    public class TransactionResponse
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public required string AccountName { get; set; }
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public TransactionDirection TransactionDirection { get; set; }
        public Guid? TransferId { get; set; }
        public string? Merchant { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}