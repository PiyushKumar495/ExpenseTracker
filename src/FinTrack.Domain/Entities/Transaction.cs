using FinTrack.Domain.Common;
using FinTrack.Domain.Enums;
namespace FinTrack.Domain.Entities
{
    public class Transaction:BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }=null!;
        public Guid AccountId { get; set; }
        public Account Account { get; set; }=null!;
        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }
        public Guid? TransferId { get; set; }//if user transfer money from one to another account
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public TransactionDirection TransactionDirection { get; set; }
        public string? Merchant { get; set; }
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool IsActive { get; set; }=true;

    }
}