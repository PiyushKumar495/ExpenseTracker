using FinTrack.Domain.Enums;
namespace FinTrack.Application.DTOs.Transactions
{
    public class CreateTransactionRequest
    {
        public Guid AccountId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? TransferId { get; set; }//if user transfer money from one to another account
        public decimal Amount { get; set; }
        public TransactionDirection TransactionDirection { get; set; }
        public string? Merchant { get; set; }
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}